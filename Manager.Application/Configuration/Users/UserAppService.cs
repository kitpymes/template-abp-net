using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Manager.Application.Configuration.Users.Dto;
using Microsoft.AspNet.Identity;
using Manager.Core.Configuration.Users;
using System.Text;
using Abp.UI;
using System;
using Manager.Core.Common.Authorization;
using Abp.Domain.Uow;
using System.Configuration;
using System.Net.Mail;
using Manager.Core.Common.Mail;

namespace Manager.Application.Configuration.Users
{
    /* THIS IS JUST A SAMPLE. */
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : ManagerAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IEmailManager _emailManager;

        public UserAppService(IRepository<User, long> userRepository, IPermissionManager permissionManager, IEmailManager emailManager)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
			_emailManager = emailManager;
        }

        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                users.MapTo<List<UserListDto>>());
        }
        public async Task CreateUser(CreateUserInput input)
        {
            var user = input.MapTo<User>();

            user.TenantId = AbpSession.TenantId;
            user.Password = new PasswordHasher().HashPassword(input.Password);
            user.IsEmailConfirmed = true;

            CheckErrors(await UserManager.CreateAsync(user));
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
        }

        public async Task ConfirmEmail(ConfirmEmailInput input)
        {
            await UserManager.ConfirmEmailAsync(input.UserId, input.ConfirmationCode);
        }
    }
}