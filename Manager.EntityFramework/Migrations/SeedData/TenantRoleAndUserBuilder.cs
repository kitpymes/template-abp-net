using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Authorization.Roles;
using Abp.MultiTenancy;
using Manager.Core.Configuration.Roles;
using Manager.EntityFramework;
using Manager.Core.Configuration.Users;
using Manager.Core.Common.Authorization;

namespace Manager.Migrations.SeedData
{
    public class TenantRoleAndUserBuilder
    {
        private readonly ManagerDbContext _context;
        private readonly int _tenantId;
        private readonly string _roleName;
        private readonly bool _isStatic;
        private readonly string _password;
        private readonly string _email;

        public TenantRoleAndUserBuilder(ManagerDbContext context, int tenantId, string email, string roleName, string password = User.DefaultPassword,  bool isStatic = true)
        {
            _context = context;
            _tenantId = tenantId;
            _email = email;
            _roleName = roleName;
            _isStatic = isStatic;
            _password = password;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == _roleName);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(
                    _tenantId, _roleName,
                    _roleName) {
                    IsStatic = _isStatic
                });

                _context.SaveChanges();

                //Grant all permissions to admin role
                var permissions = PermissionFinder
                    .GetAllPermissions(new ManagerAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();

                foreach (var permission in permissions)
                {
                     _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            TenantId = _tenantId,
                            Name = permission.Name,
                            IsGranted = _roleName == StaticRoleNames.Tenants.Admin,
                            RoleId = adminRole.Id
                        });
                  }

                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.FirstOrDefault(u => u.TenantId == _tenantId && u.UserName.ToLower() == _roleName.ToLower());
            if (adminUser == null)
            {
                adminUser = User.CreateTenantUser(_tenantId, _roleName, _email, _password);
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }
    }
}