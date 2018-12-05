using Abp.Authorization.Users;
using Abp.AutoMapper;
using Manager.Application.Account.ForgotPassword.Dto;
using Manager.Core.Configuration.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Web.Models.Account.ForgotPassword
{
    [AutoMap(typeof(ForgotPasswordLinkDtoInput))]
    public class ForgotPasswordLinkViewModel : ViewModelBase
    {
        public string EmailAddress { get; set; }

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

            return list;
        }
    }
}