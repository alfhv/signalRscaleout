using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR.ServerCore.Console.Hubs
{
    public class SimpleHub : Hub
    {
        public async Task Send(string msgType, string message)
        {
            if (Clients.All != null)
                await Clients.All.SendAsync("BroadcastMessage", msgType, message);
        }

        public async Task Echo(string msg)
        {
            Clients.Client(Context.ConnectionId).SendAsync("echo", msg);
        }
    }
}