using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR.ServerCore.Console.Hubs
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }

    public interface INotifyHub
    {
        Task Send(string msgType, string message);
    }

    public class NotifyHub : Hub<ITypedHubClient>, INotifyHub
    {
        public async Task Send(string msgType, string message)
        {
            System.Console.WriteLine($"Clients: {Clients}");
            System.Console.WriteLine($"Clients.All: {Clients.All}");
            await Clients.All.BroadcastMessage(msgType, message);
        }
    }
}
