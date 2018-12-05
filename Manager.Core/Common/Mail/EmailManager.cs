using System;
using System.Net.Mail;
using Castle.Core.Logging;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Net.Mail;
using System.Text;

namespace Manager.Core.Common.Mail
{
    //TODO: Get setting from configuration
    /// <summary>
    /// Implements <see cref="IEmailService"/> to send emails using current settings.
    /// </summary>
    public class EmailManager : IEmailManager
    {
        public ILogger Logger { get; set; }
        private ISettingManager _settingManager;

        /// <summary>
        /// Creates a new instance of <see cref="EmailManager"/>.
        /// </summary>
        public EmailManager(ISettingManager settingManager)
        {
            Logger = NullLogger.Instance;
            _settingManager = settingManager;
        }

        /// <summary>
        /// Envio de email asincrono.
        /// </summary>
        /// <param name="mail">Email a enviar.</param>
        /// <returns></returns>
        public async Task SendEmailAsync(MailMessage mail)
        {
            try
            {
                var address = await _settingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
                var displayName = await _settingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName);
                var host = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
                var port = Convert.ToInt32(await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Port));
                bool useDefaultCredentials = Convert.ToBoolean(await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UseDefaultCredentials));
                var email = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
                var pass = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

                mail.From = new MailAddress(address, displayName);
                using (var client = new SmtpClient(host, port))
                {
                    client.UseDefaultCredentials = useDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(email, pass);
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send email! => SendEmailAsync(MailMessage mail)", ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Envio de email asincrono.
        /// </summary>
        /// <param name="to">Email del destinatario.</param>
        /// <param name="subject">Asunto del email.</param>
        /// <param name="body">Contenido del email.</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage();
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;

            await SendEmailAsync(mail);
        }
    }
}