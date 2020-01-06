using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RequestsServiceTests
{
    public class RequestsServiceTestBase
    {
        protected Mock<IRequestsRepository> RequestsRepository { get; set; }
        protected Mock<IRelationshipsService> RelationshipsService { get; set; }

        protected IRequestsService RequestsService { get; set; }

        protected void Setup()
        {
            RequestsRepository = new Mock<IRequestsRepository>();
            RelationshipsService = new Mock<IRelationshipsService>();

            RequestsService = new RequestsService(RequestsRepository.Object, RelationshipsService.Object);
        }
    }
}
