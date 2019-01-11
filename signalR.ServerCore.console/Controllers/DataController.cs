using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using signalR.ServerCore.Clients;
using Microsoft.Extensions.Configuration;

namespace signalR.ServerCore.Console.Controllers
{
    [Route("data")]
    public class DataController : Controller
    {
        private BackplaneClient _backplaneClient;
        public DataController(BackplaneClient backplaneClient)
        {
            _backplaneClient = backplaneClient;
        }

        /// <summary>
        /// Notify all registered instances
        /// </summary>
        /// <param name="msgType">info, warn, error, success</param>
        /// <param name="msgContent"></param>
        [Route("publish")]
        [HttpPost]
        public async Task<IActionResult> PublishMessage(string msgType, string msgContent)
        {
            await _backplaneClient.Notify(msgType, msgContent);

            return Ok();
        }

    }
}