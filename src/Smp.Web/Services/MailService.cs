using System;
using System.Threading.Tasks;

namespace Smp.Web.Services
{
    public interface IMailService
    {
        Task SendEmail(string receiverEmail, string subject, string body);
    }

    public class MailService : IMailService
    {
        public MailService()
        {
            
        }

        public Task SendEmail(string rcvr, string sub, string body)
        {
            throw new NotImplementedException();
        }
    }
}