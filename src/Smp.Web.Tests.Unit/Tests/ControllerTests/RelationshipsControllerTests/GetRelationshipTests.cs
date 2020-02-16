using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RelationshipsControllerTests
{
    [TestFixture]
    public class GetRelationshipTests
    {
        [TestFixture]
        public class GivenAnExistentRelationship : RelationshipsControllerTestBase
        {
            private readonly Guid _userOneId = Guid.NewGuid();
            private readonly Guid _userTwoId = Guid.NewGuid();
            private const byte RelationshipTypeId = 1;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RelationshipsRepository.Setup(repository => repository
                        .GetRelationshipByIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RelationshipType>()))
                    .ReturnsAsync(new Relationship());

                _result = await RelationshipsController.GetRelationship(_userOneId, _userTwoId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipByIdsAndType()
                => RelationshipsRepository.Verify(
                    repository =>
                        repository.GetRelationshipByIdsAndType(_userOneId, _userTwoId,
                            (RelationshipType) RelationshipTypeId), Times.Once);
        }

        [TestFixture]
        public class GivenANonExistentRelationship : RelationshipsControllerTestBase
        {
            private readonly Guid _userOneId = Guid.NewGuid();
            private readonly Guid _userTwoId = Guid.NewGuid();
            private const byte RelationshipTypeId = 1;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RelationshipsRepository.Setup(repository => repository
                        .GetRelationshipByIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RelationshipType>()))
                    .ReturnsAsync((Relationship)null);

                _result = await RelationshipsController.GetRelationship(_userOneId, _userTwoId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<NotFoundResult>(_result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipByIdsAndType()
                => RelationshipsRepository.Verify(
                    repository =>
                        repository.GetRelationshipByIdsAndType(_userOneId, _userTwoId,
                            (RelationshipType)RelationshipTypeId), Times.Once);
        }

        [TestFixture]
        public class GivenAnUnauthorizedRequest : RelationshipsControllerTestBase
        {
            private readonly Guid _userOneId = Guid.NewGuid();
            private readonly Guid _userTwoId = Guid.NewGuid();
            private const byte RelationshipTypeId = 1;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);

                _result = await RelationshipsController.GetRelationship(_userOneId, _userTwoId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipByIdsAndType()
                => RelationshipsRepository.Verify(
                    repository =>
                        repository.GetRelationshipByIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RelationshipType>()),
                    Times.Never);
        }
    }
}
