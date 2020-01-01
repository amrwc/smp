using Moq;
using Smp.Web.Controllers;
using Smp.Web.Repositories;
using Smp.Web.Services;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.AccountsControllerTests
{
    public class AccountsControllerTestBase
    {
        protected  Mock<IUsersRepository> UsersRepository { get; set; }
        protected  Mock<IActionsRepository> ActionsRepository { get; set; }
        protected  Mock<IAccountsService> AccountsService { get; set; }
        protected  Mock<IActionValidator> ActionValidator { get; set; }
        protected Mock<ICryptographyService> CryptographyService { get; set; }

        protected AccountsController AccountsController { get; set; }

        public void Setup()
        {
            UsersRepository = new Mock<IUsersRepository>();
            ActionsRepository = new Mock<IActionsRepository>();
            AccountsService = new Mock<IAccountsService>();
            ActionValidator = new Mock<IActionValidator>();
            CryptographyService = new Mock<ICryptographyService>();

            AccountsController = new AccountsController(UsersRepository.Object, ActionsRepository.Object, AccountsService.Object, ActionValidator.Object, CryptographyService.Object);
        }
    }
}
