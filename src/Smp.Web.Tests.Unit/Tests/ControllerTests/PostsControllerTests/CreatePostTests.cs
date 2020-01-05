using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;
using Smp.Web.Models.Requests;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.PostsControllerTests
{
    [TestFixture]
    public class CreatePostTests
    {
        public class GivenAnUnauthorizedRequest : PostsControllerTestBase
        {
            private CreatePostRequest _createPostRequest;

            private IActionResult _result;

            [OneTimeSetUp]
            public async Task WhenCreatePostGetsCalled()
            {
                Setup();

                _createPostRequest = new Fixture().Create<CreatePostRequest>();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>()))
                    .Returns(false);

                _result = await PostsController.CreatePost(_createPostRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<UnauthorizedResult>(_result);
        }

        public class GivenAnAuthorizedRequest : PostsControllerTestBase
        {
            private CreatePostRequest _createPostRequest;

            private IActionResult _result;

            private Post _usedPost;

            [OneTimeSetUp]
            public async Task WhenCreatePostGetsCalled()
            {
                Setup();

                _createPostRequest = new Fixture().Create<CreatePostRequest>();

                AuthService.Setup(service => service.AuthorizeSelf(It.IsAny<string>(), It.IsAny<Guid>()))
                    .Returns(true);
                AuthService.Setup(service => service.AuthorizeFriend(It.IsAny<string>(), It.IsAny<Guid>()))
                    .ReturnsAsync(true);
                PostsService.Setup(service => service.CreatePost(It.IsAny<Post>()))
                    .Callback<Post>(post => _usedPost = post);

                _result = await PostsController.CreatePost(_createPostRequest);
            }

            [Test]
            public void ThenResultShouldBeOfExpectedType()
                => Assert.IsInstanceOf<OkResult>(_result);

            [Test]
            public void ThenUsedPostShouldBeAsExpected()
                => _usedPost.Should().BeEquivalentTo(new Post(_createPostRequest), options => options.Excluding(post => post.CreatedAt).Excluding(post => post.Id));

            [Test]
            public void ThenPostsServiceCreatePostShouldHaveBeenCalled()
                => PostsService.Verify(service => service.CreatePost(It.IsAny<Post>()), Times.Once);
        }
    }
}
