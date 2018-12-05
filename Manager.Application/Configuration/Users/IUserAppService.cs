using System.Threading.Tasks;
using Abp.Application.Services;
using Manager.Application.Configuration.Users.Dto;
using Manager.Core.Configuration.Users;
using Abp.Application.Services.Dto;

namespace Manager.Application.Configuration.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsers();

        Task CreateUser(CreateUserInput input);

		Task ConfirmEmail(ConfirmEmailInput input);
    }
}