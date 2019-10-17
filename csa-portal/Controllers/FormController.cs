using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Csa.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gov.Pssg.Csa.Public.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly ILogger<FormController> _logger;

        public FormController(ILogger<FormController> logger)
        {
            _logger = logger;
        }

        // GET: form/activities
        [Route("Activities")]
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            try
            {
                var data = await Form.GetActivitiesAsync();
                _logger.LogInformation("Successfully retrieved activities {@Activities}", data);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve activities");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Form form)
        {
            try
            {
                // validate form
                bool validationResult = await form.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for form {@Form}", form);
                    return BadRequest();
                }

                // TODO: submit form to Dynamics
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to submit form {@Form}", form);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
