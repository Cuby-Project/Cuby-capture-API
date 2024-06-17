using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Controller : ControllerBase
    {

        private readonly ILogger<Controller> _logger;

        public Controller(ILogger<Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public OkResult Get()
        {
            return Ok();
        }
    }
}
