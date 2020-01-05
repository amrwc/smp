using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.PostsServiceTests
{
    [TestFixture]
    public class GetPostsByReceiverIdTests
    {
        public class GivenAValidCall : PostsServiceTestBase
        {
            private const int Count = 15;
            private readonly Guid _userId = Guid.NewGuid();

            private IList<Post> _posts;
            private IList<Post> _result;

            [OneTimeSetUp]
            public async Task WhenGetPostsByReceiverIdGetsCalled()
            {
                Setup();

                _posts = new Fixture().CreateMany<Post>().ToList();
                PostsRepository.Setup(repository => repository.GetPostsByReceiverId(It.IsAny<Guid>(), It.IsAny<int>()))
                    .ReturnsAsync(_posts);

                _result = await PostsService.GetPostsByReceiverId(_userId, Count);
            }

            [Test]
            public void ThenPostsRepositoryGetPostsByReceiverIdShouldHaveBeenCalled()
                => PostsRepository.Verify(repository => repository.GetPostsByReceiverId(_userId, Count), Times.Once);

            [Test]
            public void ThenResultShouldBeAsExpected()
                => _result.Should().BeEquivalentTo(_posts);
        }
    }
}
