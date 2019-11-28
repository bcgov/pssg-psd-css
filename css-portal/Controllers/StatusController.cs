using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest.Models;
using Gov.Pssg.Css.Public.Utility;
using Gov.Pssg.Css.Public.ViewModels;
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
        private readonly ILogger<ComplaintsController> _logger;
        private readonly IConfiguration _configuration;

        public StatusController(ILogger<ComplaintsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: status/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var status = new Status
                {
                    CaptchaApiUrl = _configuration["CAPTCHA_API_URL"],
                };

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
