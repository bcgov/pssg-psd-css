using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gov.Pssg.Css.Public.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly ILogger<ComplaintController> _logger;

        public ComplaintController(ILogger<ComplaintController> logger)
        {
            _logger = logger;
        }

        // GET: complaint/propertyTypes
        [Route("PropertyTypes")]
        [HttpGet]
        public async Task<IActionResult> GetPropertyTypes()
        {
            try
            {
                var data = await PropertyType.GetPropertyTypesAsync();
                _logger.LogInformation("Successfully retrieved property types {@PropertyTypes}", data);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve property types");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Complaint complaint)
        {
            try
            {
                // validate complaint
                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                // TODO: submit complaint to Dynamics
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to submit complaint {@Complaint}", complaint);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
