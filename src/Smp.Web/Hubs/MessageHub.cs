using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Smp.Web.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        public async Task SendMessage(string channelName, object data = null)
        {
            await Clients.All.SendAsync($"{channelName}", data);
        }
    }
}