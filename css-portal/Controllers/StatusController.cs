using System;
using Gov.Pssg.Css.Public.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gov.Pssg.Css.Public.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly IConfiguration _configuration;

        public StatusController(ILogger<StatusController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: status/
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Attempting to retrieve status");
            try
            {
                var status = StatusUtility.GetStatus(_configuration);
                _logger.LogInformation("Successfully retrieved {@Status}", status);
                return new JsonResult(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve status");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
