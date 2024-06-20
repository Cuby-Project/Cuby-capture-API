using Contract.services;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CubeController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ControllerBase> _logger;

        /// <summary>
        /// Service to solve the cube
        /// </summary>
        private readonly IKociembaCore _service;

        public CubeController(ILogger<CubeController> logger, IKociembaCore service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Solves the cube
        /// </summary>
        /// <param name="cube"></param>
        /// <returns>cube solve string</returns>
        [HttpGet("solve", Name = "solve")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string cube)
        {
            try
            {
                if (_service.CheckValidity(cube).Result)
                {
                    return BadRequest();
                }
                return Ok(await _service.Solve(cube));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while solving the cube");
                return Problem();
            }
        }
    }
}
