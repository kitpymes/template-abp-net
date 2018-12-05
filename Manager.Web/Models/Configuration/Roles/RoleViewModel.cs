using Abp.AutoMapper;
using Manager.Application.Configuration.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Manager.Web.Models.Configuration.Roles
{
    [AutoMap(typeof(RoleDto))]
    public class RoleViewModel : ViewModelBase
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return list;
        }
    }
}