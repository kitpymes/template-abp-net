using Abp.AutoMapper;
using Manager.Application.Configuration.Roles.Dto;
using Manager.Core.Configuration.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Web.Models.Configuration.Roles
{
    [AutoMap(typeof(UpdateRoleDtoInput))]
    public class UpdateRoleViewModel : ViewModelBase
    {
        public long Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public bool IsDeleted { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            list = new List<ValidationResult>();

            list.AddRange(ValidateInputText(
                input: Name,
                displayName: L("CONFIG.ROLES.Name"),
                maxLength: Role.MaxNameLength,
                validateMaxLength: true
            ));

            list.AddRange(ValidateInputText(
                input: DisplayName,
                displayName: L("CONFIG.ROLES.DisplayName"),
                maxLength: Role.MaxDisplayNameLength,
                validateMaxLength: true
            ));

            return list;
        }
     }
}