using System.Net.Mail;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.MailServiceTests
{
    public class SendEmailTests
    {
        public class GivenAValidEmail : MailServiceTestBase
        {
            private const string ReceiverEmail = "receiver@email.com";
            private const string EmailSubject = "Subject";
            private const string EmailBody = "Body";

            private MailMessage _expectedMailMessage;
            private MailMessage _usedMailMessage;

            [OneTimeSetUp]
            public async Task WhenSendEmailGetsCalled()
            {
                Setup();

                _expectedMailMessage = new MailMessage(new MailAddress("noreply@smp.com", "SMP"), new MailAddress(ReceiverEmail))
                {
                    Subject = EmailSubject,
                    Body = EmailBody,
                    IsBodyHtml = true
                };

                SmtpClient.Setup(client => client.SendMailAsync(It.IsAny<MailMessage>()))
                    .Callback<MailMessage>(message => _usedMailMessage = message);

                await MailService.SendEmail(ReceiverEmail, EmailSubject, EmailBody);
            }

            [Test]
            public void SmtpClientSendShouldHaveBeenCalled()
                => SmtpClient.Verify(client => client.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);

            [Test]
            public void UsedMailMessageShouldBeAsExpected()
                => _usedMailMessage.Should().BeEquivalentTo(_expectedMailMessage);
        }
    }
}
