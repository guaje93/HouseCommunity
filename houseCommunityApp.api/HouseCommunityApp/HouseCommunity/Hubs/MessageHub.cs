using System;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.Model;
using HouseCommunity.DTOs;
using System.Collections.Generic;

namespace HouseCommunity.Hubs
{
    public class MessageHub : Hub
    {
        public async Task NewMessage(string groupName ,object msg)
        {
            await Clients.Group(groupName).SendAsync("MessageReceived", msg);
        }

        public Task JoinGroup(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveGroup(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
