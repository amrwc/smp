using System.Net.Mail;
using System.Threading.Tasks;

namespace Smp.Web.Wrappers
{
    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage email);
    }

    public class SmtpClientWrapper : ISmtpClient
    {
        private SmtpClient _smtpClient;

        public SmtpClientWrapper(string host, ushort port)
        {
            _smtpClient = new SmtpClient(host, port);
        }

        public async Task SendMailAsync(MailMessage email)
            => await _smtpClient.SendMailAsync(email);
    }
}