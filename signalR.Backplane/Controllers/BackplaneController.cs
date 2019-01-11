using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using signalR.Backplane.Services;

namespace signalR.Backplane.Controllers
{
    [Route("backplane")]
    [ApiController]
    public class BackplaneController : Controller
    {
        private readonly IBackplaneNotificationService _notificationService; 

        public BackplaneController(IBackplaneNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("ping")]
        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }

        [Route("addinstance")]
        [HttpPost]
        public ActionResult<int> AddInstance(string urlInstance)
        {
            _notificationService.AddInstance(urlInstance);

            return Ok(_notificationService.GetInstances().Count());
        }

        [Route("publish")]
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PublishMessage(string msgType, string msgContent)
        {
            await _notificationService.Publish(msgType, msgContent);

            return Ok();
        }        

        [Route("getinstances")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetInstances()
        {
            return Ok(_notificationService.GetInstances());
        }
    }
}
