using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.RelationshipsServiceTests
{
    [TestFixture]
    public class AddFriendTests
    {
        public class GivenAValidCall : RelationshipsServiceTestBase
        {
            private readonly Guid _userOneId = Guid.NewGuid();
            private readonly Guid _userTwoId = Guid.NewGuid();

            private Relationship _relationship;
            private Relationship _usedRelationship;

            [OneTimeSetUp]
            public async Task WhenAddFriendGetsCalled()
            {
                Setup();

                _relationship = new Relationship(_userOneId, _userTwoId, RelationshipType.Friend);

                RelationshipsRepository.Setup(repository => repository.CreateRelationship(It.IsAny<Relationship>()))
                    .Callback<Relationship>(rel => _usedRelationship = rel);

                await RelationshipsService.AddFriend(_userOneId, _userTwoId);
            }

            [Test]
            public void ThenRelationshipsRepositoryAddRelationshipShouldHaveBeenCalled()
                => RelationshipsRepository.Verify(repository => repository.CreateRelationship(It.IsAny<Relationship>()), Times.Once);

            [Test]
            public void ThenUsedRelationshipShouldBeAsExpected()
                => _usedRelationship.Should().BeEquivalentTo(_relationship, options => options.Excluding(relationship => relationship.CreatedAt));
        }
    }
}
