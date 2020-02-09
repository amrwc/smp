using Moq;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ServiceTests.ConversationsServiceTests
{
    public class ConversationsServiceTestBase
    {
        protected Mock<IConversationsRepository> ConversationsRepository { get; set; }

        protected IConversationsService ConversationsService { get; set; }

        protected void Setup()
        {
            ConversationsRepository = new Mock<IConversationsRepository>();

            ConversationsService = new ConversationsService(ConversationsRepository.Object);
        }
    }
}
