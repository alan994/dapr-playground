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
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get([FromServices] DaprClient daprClient)
        {
            await daprClient.SaveStateAsync("conversation.api1", "pero", "alan");

            var value = daprClient.GetStateAsync<string>("conversation.api1", "pero");

            return "Dominik: " + value;
        }
    }
}
