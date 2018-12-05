using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Manager.EntityFramework;
using Microsoft.AspNet.Identity;
using Manager.Core.Common.Authorization;
using Manager.Core.Configuration.Roles;
using Manager.Core.Configuration.Users;

namespace Manager.Migrations.SeedData
{
    public class HostRoleAndUserCreator
    {
        private readonly ManagerDbContext _context;
        private readonly string _roleName;
        private readonly string _password;
        private readonly string _email;

        public HostRoleAndUserCreator(ManagerDbContext context, string email, string roleName, string password = User.DefaultPassword)
        {
            _context = context;
            _roleName = roleName;
            _password = password;
            _email = email;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            //Master ROLE for host
            var masterRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == _roleName);
            if (masterRoleForHost == null)
            {
                masterRoleForHost = _context.Roles.Add(new Role {
                    Name = _roleName,
                    DisplayName = _roleName,
                    IsStatic = true });
                _context.SaveChanges();

                //Grant all tenant permissions
                var permissions = PermissionFinder
                    .GetAllPermissions(new ManagerAuthorizationProvider())
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
                    .ToList();

                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = masterRoleForHost.Id
                        });
                }

                _context.SaveChanges();
            }

            //Master USER for tenancy host
            var masterUserForHost = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName.ToLower() == _roleName.ToLower());
            if (masterUserForHost == null)
            {
                masterUserForHost = _context.Users.Add(
                    new User
                    {
                        UserName = _roleName,
                        Name = _roleName,
                        Surname = _roleName,
                        EmailAddress = _email,
                        IsEmailConfirmed = true,
                        Password = new PasswordHasher().HashPassword(_password)
                    });

                _context.SaveChanges();

                _context.UserRoles.Add(new UserRole(null, masterUserForHost.Id, masterRoleForHost.Id));

                _context.SaveChanges();
            }
        }
    }
}