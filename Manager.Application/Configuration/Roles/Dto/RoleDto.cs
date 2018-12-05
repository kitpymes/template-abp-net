using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Manager.Core.Configuration.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Application.Configuration.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleDto : EntityDto
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }
    }
}
