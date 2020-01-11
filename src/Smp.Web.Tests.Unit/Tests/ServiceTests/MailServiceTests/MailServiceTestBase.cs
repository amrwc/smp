using Moq;
using Smp.Web.Services;
using Smp.Web.Wrappers;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.MailServiceTests
{
    public class MailServiceTestBase
    {
        protected const string EmailTemplate = "{{Name}}|{{Link}}";

        protected Mock<ISmtpClient> SmtpClient { get; set; }
        protected Mock<IFileWrapper> FileWrapper { get; set; }

        protected IMailService MailService { get; set; }

        protected void Setup()
        {
            SmtpClient = new Mock<ISmtpClient>();
            FileWrapper = new Mock<IFileWrapper>();

            FileWrapper.Setup(wrapper => wrapper.ReadAllText(@".\Resources\Email\email-template.html")).Returns(EmailTemplate);

            MailService = new MailService(SmtpClient.Object, FileWrapper.Object);
        }
    }
}
