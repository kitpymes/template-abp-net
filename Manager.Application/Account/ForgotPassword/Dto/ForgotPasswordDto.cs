using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Manager.Core.Configuration.Users;
using AutoMapper;

namespace Manager.Application.Account.ForgotPassword.Dto
{
    [AutoMap(typeof(User))]
    public class ForgotPasswordDto : EntityDto
    {
        public int? TenantId { get; set; }
        public string EmailAddress { get; set; }
    }
}
