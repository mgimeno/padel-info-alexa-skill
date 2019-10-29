using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace lta_padel.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly IHostingEnvironment _environment;

        public StatusController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Status()
        {
            return Ok($"API IS READY. Environment: {_environment.EnvironmentName}");
        }
    }
}
