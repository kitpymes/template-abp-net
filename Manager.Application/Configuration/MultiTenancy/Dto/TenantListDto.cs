using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Manager.Core.Configuration.MultiTenancy;

namespace Manager.Application.Configuration.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantListDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string ConnectionString { get; set; }
    }
}