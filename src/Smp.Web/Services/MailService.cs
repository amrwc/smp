using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Smp.Web.Wrappers;

namespace Smp.Web.Services
{
    public interface IMailService
    {
        Task SendEmail(string receiverEmail, string subject, string body);
    }

    public class MailService : IMailService
    {
        private const string EmailTemplate = 
        @"<h1>SMP</h1>
        <p>Hello {{Name}}</p>,
        <p>{{Content}}.</p>";
        // Use templater e.g. Razor, scriban, StringTemplate, SmartFormat, etc.

        private ISmtpClient _smtpClient;

        public MailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendEmail(string rcvr, string sub, string body)
        {
            var email = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(rcvr))
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(email);
        }
    }
}