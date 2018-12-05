﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Manager.Application.Configuration.Roles.Dto
{
    public class UpdateRolePermissionsInput : EntityDto
    {
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }

        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}