using System;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Model;

namespace HouseCommunity.Hubs
{
    public class MessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
