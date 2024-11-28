using Microsoft.AspNetCore.Mvc;

namespace Cuby.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController(ILogger<IndexController> logger) : ControllerBase
    {
        [HttpGet(Name = "GetTest")]
        public IActionResult Get()
        {
            try
            {
                logger.LogInformation("Get request received");
                return Ok();
            } catch (Exception)
            {
                logger.LogError("An error occurred while processing the request");
                return Problem();
            }
        }
    }
}
