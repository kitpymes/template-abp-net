using System.Threading.Tasks;
using Abp.Application.Services;
using Manager.Application.Configuration.Profile.Dto;

namespace Manager.Application.Configuration.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ResultType> UpdateProfileAsync(UpdateProfileDtoInput input);
    }
}