using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RelationshipsServiceTests
{
    [TestFixture]
    public class AreAlreadyFriendsTests
    {
        public class GivenTwoFriends : RelationshipsServiceTestBase
        {
            private readonly Guid _userOneId = Guid.NewGuid();
            private readonly Guid _userTwoId = Guid.NewGuid();

            private bool _result;

            [OneTimeSetUp]
            public async Task WhenAreAlreadyFriendsGetsCalled()
            {
                Setup();
                
                RelationshipsRepository.Setup(repository =>
                    repository.GetRelationshipByIdsAndType(It.IsAny<Guid>(), It.IsAny<Guid>(),
                        It.IsAny<RelationshipType>())).ReturnsAsync(new Relationship());

                _result = await RelationshipsService.AreAlreadyFriends(_userOneId, _userTwoId);
            }

            [Test]
            public void ThenResultShouldBeAsExpected()
                => Assert.IsTrue(_result);

            [Test]
            public void ThenRelationshipsRepositoryGetRelationshipByIdsAndTypeShouldHaveBeenCalled()
                => RelationshipsRepository.Verify(
                    repository =>
                        repository.GetRelationshipByIdsAndType(_userOneId, _userTwoId, RelationshipType.Friend),
                    Times.Once);
        }
    }
}
