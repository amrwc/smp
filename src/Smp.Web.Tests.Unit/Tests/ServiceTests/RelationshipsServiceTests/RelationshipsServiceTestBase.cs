using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RelationshipsServiceTests
{
    public class RelationshipsServiceTestBase
    {
        protected Mock<IRelationshipsRepository> RelationshipsRepository { get; set; }

        protected RelationshipsService RelationshipsService { get; set; }

        protected void Setup()
        {
            RelationshipsRepository = new Mock<IRelationshipsRepository>();

            RelationshipsService = new RelationshipsService(RelationshipsRepository.Object);
        }
    }
}
