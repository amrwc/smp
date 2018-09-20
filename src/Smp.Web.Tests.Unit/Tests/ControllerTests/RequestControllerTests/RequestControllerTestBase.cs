using Moq;
using Smp.Web.Controllers;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestControllerTests
{
    public class RequestControllerTestBase
    {
        protected Mock<IRequestService> RequestService;

        protected RequestController RequestController;

        public void Setup()
        {
            RequestService = new Mock<IRequestService>();

            RequestController = new RequestController(RequestService.Object);
        }
    }
}
