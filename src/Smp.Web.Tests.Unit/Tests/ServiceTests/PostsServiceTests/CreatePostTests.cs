using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Smp.Web.Models;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.PostsServiceTests
{
    [TestFixture]
    public class CreatePostTests
    {
        public class GivenAValidCall : PostsServiceTestBase
        {
            private Post _post;

            [OneTimeSetUp]
            public async Task WhenCreatePostGetsCalled()
            {
                Setup();

                _post = new Fixture().Create<Post>();

                await PostsService.CreatePost(_post);
            }

            [Test]
            public void PostsRepositoryCreatePostShouldHaveBeenCalled()
                => PostsRepository.Verify(repository => repository.CreatePost(_post), Times.Once);
        }
    }
}
