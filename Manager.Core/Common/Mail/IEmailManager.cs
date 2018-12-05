using System.Net.Mail;
using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Manager.Core.Common.Mail
{
    /// <summary>
    /// This service is used to send emails.
    /// </summary>
    public interface IEmailManager : IDomainService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="mail">Email to be sent</param>
        /// <returns></returns>
        Task SendEmailAsync(MailMessage mail);

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task SendEmailAsync(string to, string subject, string body);
    }
}