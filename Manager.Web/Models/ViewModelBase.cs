using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Abp.Extensions;
using Abp.Localization;
using Manager.Core;

namespace Manager.Web.Models
{
    public abstract class ViewModelBase : IValidatableObject
    {
        protected string MessageError { get; set; }

        protected List<ValidationResult> list { get; set; }

        public static string L(string name)
        {
            return LocalizationHelper.GetString(ManagerConsts.LocalizationSourceName, name);
        }

        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

        #region Validaciones comunes

        /// <summary>
        /// Valida un input text.
        /// </summary>
        /// <returns>Lista de mensajes con errores.</returns>
        protected virtual IEnumerable<ValidationResult> ValidateInputText(
            string input, 
            string displayName,
            string patternFormat = "",
            int? minLength = 0,
            int? maxLength = 0,
            bool validateFormat = false,
            bool validateMinLength = false,
            bool validateMaxLength = false)
        {
            if (input.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult(string.Format(L(ValidationHelper.Msg.Mandatory), displayName));
            }
          
            if (validateFormat && !patternFormat.IsNullOrWhiteSpace())
            {
                MessageError = ValidationHelper.IsValidFormat(input, displayName, patternFormat);
                if (!MessageError.IsNullOrWhiteSpace())
                    yield return new ValidationResult(MessageError);
            }

            if (validateMinLength && minLength.HasValue)
            {
                MessageError = ValidationHelper.IsValidMinLength(input, displayName, minLength.Value);
                if (!MessageError.IsNullOrWhiteSpace())
                    yield return new ValidationResult(MessageError);
            }

            if (validateMaxLength && maxLength.HasValue)
            {
                MessageError = ValidationHelper.IsValidMaxLength(input, displayName, maxLength.Value);
                if (!MessageError.IsNullOrWhiteSpace())
                    yield return new ValidationResult(MessageError);
            }
            
        }

        #endregion
    }
}