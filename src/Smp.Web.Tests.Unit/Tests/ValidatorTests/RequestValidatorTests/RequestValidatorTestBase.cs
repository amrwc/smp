using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;
using Smp.Web.Validators;

namespace Smp.Web.Tests.Unit.Tests.ValidatorTests.RequestValidatorTests
{
    public class RequestValidatorTestBase
    {
        protected Mock<IRequestsRepository> RequestsRepository { get; set; }
        protected Mock<IRelationshipsService> RelationshipsService { get; set; }
        protected Mock<IRequestsService> RequestsService { get; set; }

        protected IRequestValidator RequestValidator { get; set; }

        protected void Setup()
        {
            RequestsRepository = new Mock<IRequestsRepository>();
            RelationshipsService = new Mock<IRelationshipsService>();
            RequestsService = new Mock<IRequestsService>();

            RequestValidator = new RequestValidator(RequestsRepository.Object, RelationshipsService.Object, RequestsService.Object);
        }
    }
}
