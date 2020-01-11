using System.Net.Mail;
using System.Threading.Tasks;
using Scriban;
using Smp.Web.Wrappers;

namespace Smp.Web.Services
{
    public interface IMailService
    {
        Task SendEmail(string receiverEmail, string subject, string body);
        Task SendResetPasswordEmail(string receiverEmail, string name, string resetLink);
    }

    public class MailService : IMailService
    {
        private readonly ISmtpClient _smtpClient;
		private readonly string _emailTemplate;

        public MailService(ISmtpClient smtpClient, IFileWrapper fileWrapper)
        {
            _smtpClient = smtpClient;
			_emailTemplate = fileWrapper.ReadAllText(@".\Resources\Email\email-template.html");
        }

        public async Task SendEmail(string receiverEmail, string sub, string body)
        {
            var email = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(receiverEmail))
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(email);
        }

        public async Task SendResetPasswordEmail(string receiverEmail, string name, string resetLink)
        {
            var template = Template.Parse(_emailTemplate);
            var body = await template.RenderAsync(new { Name = name, Link = resetLink });

            var email = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(receiverEmail))
            {
                Subject = "SMP - Password Reset",
                IsBodyHtml = true
            };

            var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            var bgTop = new LinkedResource(@"Resources/Email/bg_top.jpg") { ContentId = "bg_top" };
            var bgBottom = new LinkedResource(@"Resources/Email/bg_bottom.jpg") { ContentId = "bg_bottom" };
            var smpLogo = new LinkedResource(@"Resources/Email/smp-logo.png") { ContentId = "smplogo" };
            var instagram = new LinkedResource(@"Resources/Email/instagram2x.png") { ContentId = "instagram" };
            var linkedin = new LinkedResource(@"Resources/Email/linkedin2x.png") { ContentId = "linkedin" };
            var twitter = new LinkedResource(@"Resources/Email/twitter2x.png") { ContentId = "twitter" };

			view.LinkedResources.Add(bgTop);
			view.LinkedResources.Add(bgBottom);
			view.LinkedResources.Add(smpLogo);
			view.LinkedResources.Add(instagram);
			view.LinkedResources.Add(linkedin);
			view.LinkedResources.Add(twitter);

			email.AlternateViews.Add(view);

            await _smtpClient.SendMailAsync(email);
        }
    }
}