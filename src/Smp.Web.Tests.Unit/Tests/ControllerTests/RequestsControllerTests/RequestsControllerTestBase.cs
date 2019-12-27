using Moq;
using Smp.Web.Controllers;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RequestsControllerTests
{
    public class RequestsControllerTestBase
    {
        protected Mock<IRequestsRepository> RequestsRepository;
        protected Mock<IRequestsService> RequestsService;

        protected RequestsController RequestsController;

        public void Setup()
        {
            RequestsRepository = new Mock<IRequestsRepository>();
            RequestsService = new Mock<IRequestsService>();

            RequestsController = new RequestsController(RequestsService.Object, RequestsRepository.Object);
        }
    }
}
