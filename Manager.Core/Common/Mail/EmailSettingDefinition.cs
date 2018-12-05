using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;

namespace Manager.Core.Common.Mail
{
    public class EmailSettingDefinition
    {
        public static class MailConfig
        {
            public const string port = "25";
            public const string host = "mail.easyrent.software";
            public const string enableSsl = "true";
            public const string useDefaultCredentials = "true";
            public const string defaultFromDisplayName = "Manager easyrent.software";
            public const string domain = "";

            public class Address
            {
                public const string no_reply = "no-reply@easyrent.software";
                public const string no_reply_pass = "Millon01";

                public const string hello = "hello@easyrent.software";
                public const string hello_pass = "Millon01";
            }
        }
    }

    /// <summary>
    /// Provide settings for email.
    /// </summary>
    //public class EmailSettingDefinitionProvider : SettingProvider
    //{
    //    public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
    //    {
    //        return new[]
    //               {
    //                    new SettingDefinition(EmailSettingNames.Smtp.Host, MailConfig.host, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.Port, MailConfig.port, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.UserName, MailConfig.Address.no_reply, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.Password, MailConfig.Address.no_reply_pass, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.Domain, MailConfig.domain, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, MailConfig.enableSsl, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, MailConfig.useDefaultCredentials, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.DefaultFromAddress, MailConfig.Address.no_reply, null, scopes: SettingScopes.Tenant | SettingScopes.Application),
    //                    new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, MailConfig.defaultFromDisplayName, null, scopes: SettingScopes.Tenant | SettingScopes.Application)
    //               };
    //    }

    //    private static LocalizableString L(string name)
    //    {
    //        return new LocalizableString(name, ManagerConsts.LocalizationSourceName);
    //    }
    //}
}
