using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest.Models;
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
        private readonly IDynamicsClient _dynamicsClient;

        public ComplaintsController(ILogger<ComplaintsController> logger, IDynamicsClient dynamicsClient)
        {
            _logger = logger;
            _dynamicsClient = dynamicsClient;
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
                complaint.LegislationType = Constants.LegislationTypeCSA;

                // validate complaint
                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                await SubmitComplaintToDynamicsAsync(complaint);
                return Ok();
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
                complaint.LegislationType = Constants.LegislationTypeCCLA;

                // validate complaint
                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                await SubmitComplaintToDynamicsAsync(complaint);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to submit complaint {@Complaint}", complaint);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task SubmitComplaintToDynamicsAsync(Complaint complaint)
        {
            try
            {
                var result = await DynamicsUtility.CreateComplaintAsync(_dynamicsClient, complaint);
                _logger.LogInformation("Successfully created complaint {ComplaintNumber} from view model {@Complaint}", result.CsuName, complaint);
            }
            catch (OdataerrorException ex)
            {
                _logger.LogError(ex, string.Join(Environment.NewLine, "Failed to create complaint", "{@ErrorBody}"), ex.Body);
                throw;
            }
        }
    }
}
