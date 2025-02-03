using Cuby.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cuby.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController(ILogger<IndexController> logger, IRequestService service) : ControllerBase
    {
        [HttpGet(Name = "init a request")]
        public async Task<ActionResult<string>> InitSolveRequest()
        {
            try
            {
                string id = await service.InitiateRequest();
                return Ok(id);
            } catch (Exception e)
            {
                logger.LogError(e, "Initiate request throws an error");
                return Problem("error during the request initialisation proccess");
            }
        }

        [HttpPost(Name = "Recieve cube pictures from user")]
        public async Task<ActionResult> RecieveCubePictures()
        {
            try
            {
                return Ok();
            } catch (Exception e)
            {
                logger.LogError(e, "Recieve cube pictures request throws an error");
                return Problem("error during the recieve cube pictures proccess");
            }
        }

    }
}
