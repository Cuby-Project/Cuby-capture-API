using Cuby.Data.dto;
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
    [Route("api")]
    public class IndexController(ILogger<IndexController> logger, IRequestService service) : ControllerBase
    {
        /// <summary>
        /// Endpoint to initialize a new solve request
        /// </summary>
        /// <returns>the request ID</returns>
        [HttpGet("init", Name = "InitSolveRequest")]
        [Produces("text/plain")]    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> InitSolveRequest()
        {
            try
            {
                string id = await service.InitiateRequest();
                await service.AddStepDone(id, RequestSteps.WaitingForUserCapture);
                return Ok(id);
            } catch (Exception e)
            {
                logger.LogError(e, "IndexController.InitSolveRequest() Initiate request throws an error");
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
