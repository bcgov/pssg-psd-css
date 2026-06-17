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
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IConfiguration _configuration;

        public ConfigurationController(
            ILogger<ConfigurationController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Get configuration values required by the client (frontend) application.
        /// </summary>
        /// <remarks>
        /// Should not include any sensitive values (e.g. secrets, connection strings, etc.).
        /// </remarks>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve configuration");

                return new JsonResult(
                    new
                    {
                        captcha = new { key = _configuration["captcha:key"] },
                        csaEnabled = StatusUtility.IsCSAEnabled(_configuration),
                        underMaintenance = StatusUtility.IsUnderMaintenance(_configuration),
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve configuration");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
