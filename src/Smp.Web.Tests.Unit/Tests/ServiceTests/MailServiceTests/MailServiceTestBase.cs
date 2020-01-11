using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Smp.Web.Services;
using Smp.Web.Wrappers;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.MailServiceTests
{
    public class MailServiceTestBase
    {
        protected Mock<ISmtpClient> SmtpClient { get; set; }
        protected Mock<IFileWrapper> FileWrapper { get; set; }

        protected IMailService MailService { get; set; }

        protected void Setup()
        {
            SmtpClient = new Mock<ISmtpClient>();
            FileWrapper = new Mock<IFileWrapper>();

            MailService = new MailService(SmtpClient.Object, FileWrapper.Object);
        }
    }
}
