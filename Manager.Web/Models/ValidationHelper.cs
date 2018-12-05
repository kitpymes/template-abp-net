using Abp.Localization;
using Manager.Core;
using System.Text.RegularExpressions;

namespace Manager.Web.Models
{
    public class ValidationHelper
    {
        public static string L(string name)
        {
            return LocalizationHelper.GetString(ManagerConsts.LocalizationSourceName, name);
        }

        public class Msg
        {
            /// <summary>
            /// Ocurrio un error inesperado, por favor intente mas tarde.
            /// </summary>
            public const string ErrorMessageUserDefault = "COMMON.MSG.EXCEPTION.ErrorMessageUserDefault";
            /// <summary>
            /// El campo {0} es obligatorio.
            /// </summary>
            public const string Mandatory = "COMMON.MSG.VALIDATION.Mandatory";
            /// <summary>
            /// El campo {0} tiene un formato invalido.
            /// </summary>
            public const string InvalidFormat = "COMMON.MSG.VALIDATION.InvalidFormat";
            /// <summary>
            /// El campo {0} debe contener como máximo {1} caracteres.
            /// </summary>
            public const string MaxLength = "COMMON.MSG.VALIDATION.MaxLength";
            /// <summary>
            /// El campo {0} debe contener como minimo {1} caracteres.
            /// </summary>
            public const string MinLength = "COMMON.MSG.VALIDATION.MinLength";
            /// <summary>
            /// El campo {0} no coincide con el campo {1}.
            /// </summary>
            public const string Match = "COMMON.MSG.VALIDATION.Match";
        }

        public static string IsValidMinLength(string input, string displayName, int minLength)
        {
            return input?.Length >= minLength ? ""
                : string.Format(L(Msg.MinLength), displayName, minLength);
        }

        public static string IsValidMaxLength(string input, string displayName, int maxLength)
        {
            return input?.Length <= maxLength ? ""
                : string.Format(L(Msg.MaxLength), displayName, maxLength);
        }

        public static string IsValidFormat(string input, string displayName, string pattern)
        {
            return input != null && new Regex(pattern).IsMatch(input) ? ""
                : string.Format(L(Msg.InvalidFormat), displayName);
        }

        public static string IsMatch(string input1, string input2, string displayName1, string displayName2)
        {
            return input1?.Trim().ToLower() == input2?.Trim().ToLower() ? ""
                : string.Format(L(Msg.Match), displayName1, displayName2);
        }
    }
}