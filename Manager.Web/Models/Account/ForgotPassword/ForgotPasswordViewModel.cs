using Abp.AutoMapper;
using Abp.Extensions;
using Manager.Application.Account.ForgotPassword.Dto;
using Manager.Core;
using Manager.Core.Configuration.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.Web.Models.Account.ForgotPassword
{
    [AutoMap(typeof(ForgotPasswordDtoInput))]
    public class ForgotPasswordViewModel : ViewModelBase
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string PasswordResetCode { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            list = new List<ValidationResult>();

            list.AddRange(ValidateInputText(
               input: Password,
               displayName: L("ACCOUNT.Password"),
               minLength: User.MinPlainPasswordLength,
               maxLength: User.MaxPlainPasswordLength,
               validateMaxLength: true,
               validateMinLength: true
            ));

            list.AddRange(ValidateInputText(
               input: PasswordRepeat,
               displayName: L("ACCOUNT.PasswordRepeat"),
               minLength: User.MinPlainPasswordLength,
               maxLength: User.MaxPlainPasswordLength,
               validateMaxLength: true,
               validateMinLength: true
            ));

            list.AddRange(ValidateInputText(
                input: PasswordResetCode,
                displayName: L("ACCOUNT.PasswordResetCode"),
                maxLength: User.MaxPasswordResetCodeLength,
                validateMaxLength: true
            ));

            list.AddRange(ValidMatchPasswod());

            return list;
        }

        /// <summary>
        /// Valida que las contraseña sean iguales.
        /// </summary>
        /// <returns>Lista de mensajes con errores.</returns>
        private IEnumerable<ValidationResult> ValidMatchPasswod()
        {
            // Compare Password & PasswordRepeat
            if (!Password.IsNullOrWhiteSpace() && !PasswordRepeat.IsNullOrWhiteSpace())
            {
                MessageError = ValidationHelper.IsMatch(Password, PasswordRepeat, L("ACCOUNT.Password"), L("ACCOUNT.PasswordRepeat"));
                if (!MessageError.IsNullOrWhiteSpace())
                    yield return new ValidationResult(MessageError);
            }
        }
    }
}