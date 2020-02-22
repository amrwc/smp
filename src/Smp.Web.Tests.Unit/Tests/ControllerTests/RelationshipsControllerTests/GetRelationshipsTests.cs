using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.RelationshipsControllerTests
{
    [TestFixture]
    public class GetRelationshipsTests
    {
        [TestFixture]
        public class GivenNoExistingRelationships : RelationshipsControllerTestBase
        {
            private const byte RelationshipTypeId = 1;
            private readonly Guid _userId = Guid.NewGuid();
            private readonly IList<Relationship> _relationships = new List<Relationship>();

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipsGetsCalled()
            {
                Setup();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RelationshipsRepository.Setup(repository => repository.GetRelationshipsByIdAndType(
                    It.IsAny<Guid>(), It.IsAny<RelationshipType>())).ReturnsAsync(_relationships);
                
                _result = await RelationshipsController.GetRelationships(_userId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<NoContentResult>(_result);

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

        [TestFixture]
        public class GivenExistingRelationships : RelationshipsControllerTestBase
        {
            private const byte RelationshipTypeId = 1;
            private readonly Guid _userId = Guid.NewGuid();
            private IList<Relationship> _relationships;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenGetRelationshipsGetsCalled()
            {
                Setup();

                _relationships = new Fixture().CreateMany<Relationship>().ToList();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>())).Returns(true);
                RelationshipsRepository.Setup(repository => repository.GetRelationshipsByIdAndType(
                    It.IsAny<Guid>(), It.IsAny<RelationshipType>())).ReturnsAsync(_relationships);
                
                _result = await RelationshipsController.GetRelationships(_userId, RelationshipTypeId);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => (_result as OkObjectResult)!.Value.Should().BeEquivalentTo(_relationships);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipsByIdAndTypeShouldHaveBeenCalled()
                => RelationshipsRepository.Verify(repository => repository.GetRelationshipsByIdAndType(
                    _userId, (RelationshipType) RelationshipTypeId), Times.Once);
        }
    }
}
