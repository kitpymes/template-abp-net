using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Manager.Application.Configuration.MultiTenancy.Dto;

namespace Manager.Application.Configuration.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
      ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
