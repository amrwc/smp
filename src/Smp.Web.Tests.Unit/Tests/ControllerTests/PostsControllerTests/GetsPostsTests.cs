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

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.PostsControllerTests
{
    [TestFixture]
    public class GetsPostsTests
    {
        public class GivenValidArguments : PostsControllerTestBase
        {
            private const int Count = 15;
            private readonly Guid _userId = Guid.NewGuid();

            private IActionResult _result;

            private IList<Post> _posts;

            [OneTimeSetUp]
            public async Task WhenGetsPostsGetsCalled()
            {
                Setup();

                _posts = new Fixture().CreateMany<Post>().ToList();

                PostsService.Setup(service => service.GetPostsByReceiverId(It.IsAny<Guid>(), It.IsAny<int>()))
                    .ReturnsAsync(_posts);

                _result = await PostsController.GetPosts(_userId, Count);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkObjectResult>(_result);

            [Test]
            public void ThenResultValueShouldBeAsExpected()
                => ((OkObjectResult) _result).Value.Should().BeEquivalentTo(_posts);

            [Test]
            public void ThenPostsServiceGetsPostsByReceiverIdShouldHaveBeenCalled()
                => PostsService.Verify(service => service.GetPostsByReceiverId(_userId, Count), Times.Once);
        }
    }
}
