using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.AccountsServiceTests
{
    public class AccountsServiceTestBase
    {
        protected Mock<IActionsRepository> ActionsRepository { get; set; }
        protected Mock<IUsersRepository> UsersRepository { get; set; }
        protected Mock<IMailService> MailService { get; set; }
        protected Mock<IConfiguration> Configuration { get; set; }

        protected IAccountsService AccountService { get; set; }

        public void Setup()
        {
            ActionsRepository = new Mock<IActionsRepository>();
            UsersRepository = new Mock<IUsersRepository>();
            MailService = new Mock<IMailService>();
            Configuration = new Mock<IConfiguration>();

            AccountService = new AccountsService(ActionsRepository.Object, UsersRepository.Object, MailService.Object, Configuration.Object);
        }
    }
}
