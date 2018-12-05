using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;

namespace Manager.Application.Account.ForgotPassword.Dto
{
    public class ForgotPasswordDtoOutput 
    {
        public ForgotPasswordDto Dto { get; set; }

        public string TenantName { get; set; }
    }
}
