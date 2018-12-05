using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Manager.Core.Configuration.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Manager.Core.Configuration.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123";
        public const int MinPlainPasswordLength = 3;
        public const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantUser(int tenantId, string roleName, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = roleName,
                Name = roleName,
                Surname = roleName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}