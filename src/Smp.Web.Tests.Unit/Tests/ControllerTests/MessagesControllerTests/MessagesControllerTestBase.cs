using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Smp.Web.Controllers;
using Smp.Web.Hubs;
using Smp.Web.Services;

namespace Smp.Web.Tests.Unit.Tests.ControllerTests.MessagesControllerTests
{
    public class MessagesControllerTestBase
    {
        protected Mock<IClientProxy> ClientProxy { get; set; }
        protected Mock<IHubClients> HubClients { get; set; }

        protected Mock<IAuthService> AuthService { get; set; }
        protected Mock<IMessagesService> MessagesService { get; set; }
        protected Mock<IConversationsService> ConversationsService { get; set; }
        protected Mock<IHubContext<MessageHub>> HubContext { get; set; }

        protected MessagesController MessagesController { get; set; }

        protected void Setup()
        {
            ClientProxy = new Mock<IClientProxy>();
            HubClients = new Mock<IHubClients>();
            HubClients.Setup(clients => clients.All).Returns(ClientProxy.Object);

            AuthService = new Mock<IAuthService>();
            MessagesService = new Mock<IMessagesService>();
            ConversationsService = new Mock<IConversationsService>();
            HubContext = new Mock<IHubContext<MessageHub>>();
            HubContext.Setup(context => context.Clients).Returns(HubClients.Object);

            MessagesController = new MessagesController(AuthService.Object, MessagesService.Object, ConversationsService.Object, HubContext.Object);
            MessagesController.ControllerContext = new ControllerContext();
            MessagesController.ControllerContext.HttpContext = new DefaultHttpContext();
            MessagesController.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCb2IgSmVua2lucyIsImp0aSI6IjAzZTE2MGMyLWZhNWItNDg0NS1hMjMwLTU5MDZlZTU1NWY1ZSIsImV4cCI6MTg5MzcwMTYyNywiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.C9_Y29A0ky2tObFpp7vyvm3vjlxSU4Tmfj_B1Mvyfh4");
        }
    }
}
