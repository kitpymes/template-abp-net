using Manager.Core.Configuration.Users;
using Manager.Web.Models.Upload;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using Abp.AutoMapper;
using Manager.Application.Configuration.Profile.Dto;

namespace Manager.Web.Models.Configuration.Profile
{
    [AutoMap(typeof(UpdateProfileDtoInput))]
    public class ProfileViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string PasswordRepeat { get; set; }

        public UploadConfigViewModel Avatar { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            list = new List<ValidationResult>();

            list.AddRange(ValidateInputText(
                input: Name,
                displayName: L("CONFIG.PROFILE.Name"),
                maxLength: User.MaxNameLength,
                validateMaxLength: true
            ));

            list.AddRange(ValidateInputText(
                input: Surname,
                displayName: L("CONFIG.PROFILE.Surname"),
                maxLength: User.MaxSurnameLength,
                validateMaxLength: true
            ));

            list.AddRange(ValidateInputText(
               input: EmailAddress,
               displayName: L("CONFIG.PROFILE.EmailAddress"),
               maxLength: User.MaxEmailAddressLength,
               patternFormat: User.EmailPattern,
               validateMaxLength: true,
               validateFormat: true
          ));

            list.AddRange(ValidateInputText(
               input: Password,
               displayName: L("CONFIG.PROFILE.Password"),
               minLength: User.MinPlainPasswordLength,
               maxLength: User.MaxPlainPasswordLength,
               validateMaxLength: true,
               validateMinLength: true
            ));

            list.AddRange(ValidateInputText(
               input: PasswordRepeat,
               displayName: L("CONFIG.PROFILE.PasswordRepeat"),
               minLength: User.MinPlainPasswordLength,
               maxLength: User.MaxPlainPasswordLength,
               validateMaxLength: true,
               validateMinLength: true
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
                MessageError = ValidationHelper.IsMatch(Password, PasswordRepeat, L("CONFIG.PROFILE.Password"), L("CONFIG.PROFILE.PasswordRepeat"));
                if (!MessageError.IsNullOrWhiteSpace())
                    yield return new ValidationResult(MessageError);
            }
        }
    }
}