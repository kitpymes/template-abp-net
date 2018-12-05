using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Manager.Core.Configuration.Users;

namespace Manager.Application.Account.ForgotPassword.Dto
{
    public class ForgotPasswordDtoInput
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string PasswordResetCode { get; set; }
    }
}
