using Cuby.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cuby.API.Controllers
{
    /// <summary>
    /// index controller
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="service">implementation of <see cref="IRequestService"/></param>
    [ApiController]
    [Route("[controller]")]
    public class IndexController(ILogger<IndexController> logger, IRequestService service) : ControllerBase
    {
        /// <summary>
        /// Endpoint to initialize a new solve request
        /// </summary>
        /// <returns>the request ID</returns>
        [HttpGet(Name = "init a request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Endpoint to recieve cube pictures from the user
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "Recieve cube pictures from user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
