using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using Manager.EntityFramework;
using static Manager.Core.Common.Mail.EmailSettingDefinition;
using Microsoft.AspNet.Identity;

namespace Manager.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly ManagerDbContext _context;

        public DefaultSettingsCreator(ManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, MailConfig.host);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, MailConfig.port);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, MailConfig.Address.no_reply);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, MailConfig.Address.no_reply_pass);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, MailConfig.domain);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, MailConfig.enableSsl);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, MailConfig.useDefaultCredentials);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, MailConfig.Address.no_reply);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, MailConfig.defaultFromDisplayName);

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "es");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}