using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalR.ServerCore.Console.Hubs;

namespace signalR.ServerCore.Console.Controllers
{
    [Route("notification")]
    public class NotificationController : Controller
    {
        //private readonly INotifyHub _notifyHub;
        private readonly Hub<ITypedHubClient> _notifyHub;
        private readonly SimpleHub _simpleHub;

        public NotificationController(//INotifyHub notifyHub)
                                      Hub<ITypedHubClient> notifyHub,
                                      SimpleHub simpleHub)
        {
            _notifyHub = notifyHub;
            _simpleHub = simpleHub;
        }

        [Route("ping")]
        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }

        /// <summary>
        /// Called from IBackplaneNotificationService to notify connected clients 
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [Route("notify")]
        [HttpPost]
        
        public async Task<IActionResult> NotifyClients(string msgType, string msgContent)
        {
            //await (_notifyHub as INotifyHub).Send("BroadcastMessage", msgContent);
            await _simpleHub.Send(msgType, msgContent);

            return Ok();
        }
    }
}
