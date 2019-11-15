using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace lta_padel.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;

        public StatusController(IWebHostEnvironment environment)
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
