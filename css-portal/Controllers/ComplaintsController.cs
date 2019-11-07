using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;
using Gov.Pssg.Css.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gov.Pssg.Css.Public.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly ILogger<ComplaintsController> _logger;

        public ComplaintsController(ILogger<ComplaintsController> logger)
        {
            _logger = logger;
        }

        // GET: complaints/property-types
        [Route("property-types")]
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

        // POST: complaints/csa
        [HttpPost]
        [Route("csa")]
        public async Task<IActionResult> PostCSA([FromBody] Complaint complaint)
        {
            try
            {
                // validate complaint
                bool validationResult = await complaint.Validate(ComplaintType.CSA);
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

        // POST: complaints/ccla
        [HttpPost]
        [Route("ccla")]
        public async Task<IActionResult> PostCCLA([FromBody] Complaint complaint)
        {
            try
            {
                // validate complaint
                bool validationResult = await complaint.Validate(ComplaintType.CCLA);
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
