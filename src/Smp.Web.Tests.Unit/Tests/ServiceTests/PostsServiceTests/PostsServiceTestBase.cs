using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.PostsServiceTests
{
    public class PostsServiceTestBase
    {
        protected Mock<IPostsRepository> PostsRepository { get; set; }

        protected PostsService PostsService { get; set; }

        protected void Setup()
        {
            PostsRepository = new Mock<IPostsRepository>();

            PostsService = new PostsService(PostsRepository.Object);
        }
    }
}
