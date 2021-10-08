using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conversation.Api1.Controllers
{
    [ApiController]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromServices] DaprClient daprClient)
        {
            await daprClient.SaveStateAsync("conversation-statestore", "pero", "Jebeno, radi!");

            var value = await daprClient.GetStateAsync<string>("conversation-statestore", "pero");

            return Json(value);
        }
    }
}
