using Abp.Authorization;
using Abp.Domain.Repositories;
using Manager.Core.Configuration.Users;
using Manager.Core.Common.Mail;
using Manager.Application.Configuration.Profile.Dto;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Manager.Application.Configuration.Profile
{
    public class ProfileAppService : ManagerAppServiceBase, IProfileAppService
    {
        public async Task<ResultType> UpdateProfileAsync(UpdateProfileDtoInput input)
        {
            try
            {
                User user = await UserManager.GetUserByIdAsync(AbpSession.UserId.Value);

                user.Name = input.Name;
                user.Surname = input.Surname;
                user.EmailAddress = input.EmailAddress;

                if (!string.IsNullOrWhiteSpace(input.Password))
                    user.Password = new PasswordHasher().HashPassword(input.Password);

                var result = await UserManager.UpdateAsync(user);

                return ResultType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ProfileAppService).ToString(), ex);
                return ResultType.Error;
            }
        }
    }
}