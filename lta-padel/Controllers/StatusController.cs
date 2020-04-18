using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace lta_padel.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;

        public StatusController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Status()
        {
            var isDebugMode = false;
            #if DEBUG
            isDebugMode = true;
            #endif

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $@"
                PADEL INFO API IS READY
                <br />
                Build Mode: {(isDebugMode ? "DEBUG (Development)" : "RELEASE (Production)")}
                <br />
                Environment: {_env.EnvironmentName.ToUpper()}"
            };
        }
    }
}
