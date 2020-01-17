using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.MailServiceTests
{
    public class SendResetPasswordEmailTests
    {
        public class GivenValidParameters : MailServiceTestBase
        {
            private const string ReceiverEmail = "real@email.com";
            private const string Name = "John Smith";
            private const string ResetLink = "https://example.com/";

            private MailMessage _usedMailMessage;

            [OneTimeSetUp]
            public async Task WhenSendResetPasswordEmailGetsCalled()
            {
                Setup();

                SmtpClient.Setup(client => client.SendMailAsync(It.IsAny<MailMessage>()))
                    .Callback<MailMessage>(msg => _usedMailMessage = msg);

                await MailService.SendResetPasswordEmail(ReceiverEmail, Name, ResetLink);
            }

            [Test]
            public void ThenSmtpClientSendEmailAsyncShouldHaveBeenCalled()
                => SmtpClient.Verify(client => client.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);

            [Test]
            public void ThenUsedMailMessageShouldBeAsExpected()
            {
                Assert.That(_usedMailMessage.AlternateViews.Count == 1 && _usedMailMessage.AlternateViews[0].LinkedResources.Count == 6);
                Assert.That(_usedMailMessage.From.Address, Is.EqualTo("noreply@smp.com"));
                Assert.That(_usedMailMessage.To.Count == 1 && _usedMailMessage.To[0].Address == ReceiverEmail);
                Assert.That(_usedMailMessage.Subject, Is.EqualTo("SMP - Password Reset"));
                Assert.That(_usedMailMessage.IsBodyHtml, Is.True);
            }
        }
    }
}
