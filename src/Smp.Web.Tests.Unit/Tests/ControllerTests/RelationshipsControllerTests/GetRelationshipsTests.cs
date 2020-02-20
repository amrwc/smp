using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RelationshipsControllerTests
{
    [TestFixture]
    public class GetRelationshipsTests
    {
        [TestFixtureSource("RelationshipLists")]
        public class GivenEmptyOrExistentRelationships : RelationshipsControllerTestBase
        {
            // Data provider
            private static readonly object[] RelationshipLists =
            {
                new object[] {new List<Relationship>(), typeof(NoContentResult)},
                new object[] {new List<Relationship> {It.IsAny<Relationship>()}, typeof(OkObjectResult)},
                new object[]
                {
                    new List<Relationship> {It.IsAny<Relationship>(), It.IsAny<Relationship>()}, typeof(OkObjectResult)
                }
            };

            private const byte RelationshipTypeId = 1;
            private readonly Guid _userId = Guid.NewGuid();
            private readonly List<Relationship> _relationships;
            private readonly Type _resultType;
            private IActionResult _result;

            public GivenEmptyOrExistentRelationships(List<Relationship> relationships, Type resultType)
            {
                _relationships = relationships;
                _resultType = resultType;
            }

            [OneTimeSetUp]
            public async Task WhenGetRelationshipsGetsCalled()
            {
                Setup();
                AuthService.Setup(service => service.AuthorizeSelf(
                    It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RelationshipsRepository.Setup(repository => repository.GetRelationshipsByIdAndType(
                    It.IsAny<Guid>(), It.IsAny<RelationshipType>())).ReturnsAsync(_relationships);
                _result = await RelationshipsController.GetRelationships(_userId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType() => Assert.IsInstanceOf(_resultType, _result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipsByIdAndTypeShouldHaveBeenCalled()
                => RelationshipsRepository.Verify(repository => repository.GetRelationshipsByIdAndType(
                    _userId, (RelationshipType) RelationshipTypeId), Times.Once);
        }

        [TestFixture]
        public class GivenAnUnauthorizedRequest : RelationshipsControllerTestBase
        {
            private const byte RelationshipTypeId = 1;
            private readonly Guid _userId = Guid.NewGuid();
            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipGetsCalled()
            {
                Setup();
                AuthService.Setup(service => service.AuthorizeSelf(
                    It.IsAny<string>(), It.IsAny<Guid>())).Returns(false);
                _result = await RelationshipsController.GetRelationships(_userId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipsByIdAndType()
                => RelationshipsRepository.Verify(repository => repository.GetRelationshipsByIdAndType(
                    _userId, (RelationshipType) RelationshipTypeId), Times.Never);
        }
    }
}
