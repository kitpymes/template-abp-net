using Abp.Authorization.Users;
using Manager.Core.Configuration.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Web.Models.Account.Login
{
    public class LoginViewModel : ViewModelBase
    {
        public string TenancyName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            list = new List<ValidationResult>();

            list.AddRange(ValidateInputText(
                input: EmailAddress, 
                displayName: L("ACCOUNT.EmailAddress"), 
                maxLength: AbpUserBase.MaxEmailAddressLength, 
                patternFormat: User.EmailPattern, 
                validateMaxLength: true, 
                validateFormat: true
            ));

            list.AddRange(ValidateInputText(
                input: Password, 
                displayName: L("ACCOUNT.Password"),
                minLength: User.MinPlainPasswordLength,
                maxLength: User.MaxPlainPasswordLength,
                validateMaxLength: true,
                validateMinLength: true
           ));

            return list;
        }
    }
}