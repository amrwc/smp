using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Smp.Web.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(Guid conversationId)
        {
            await Clients.All.SendAsync("newmessage", conversationId);
        }
    }
}