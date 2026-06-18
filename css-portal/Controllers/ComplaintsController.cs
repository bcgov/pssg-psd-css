using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EMBC.Suppliers.API.Services;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest.Models;
using Gov.Pssg.Css.Public.Attributes;
using Gov.Pssg.Css.Public.Utility;
using Gov.Pssg.Css.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Gov.Pssg.Css.Public.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [UnavailableDuringMaintenance]
    public class ComplaintsController : ControllerBase
    {
        private readonly ILogger<ComplaintsController> _logger;
        private readonly IDynamicsClient _dynamicsClient;
        private readonly IConfiguration _configuration;
        private readonly ICaptchaVerificationService _captchaVerificationService;

        public ComplaintsController(
            ILogger<ComplaintsController> logger,
            IDynamicsClient dynamicsClient,
            IConfiguration configuration,
            ICaptchaVerificationService captchaVerificationService
        )
        {
            _logger = logger;
            _dynamicsClient = dynamicsClient;
            _configuration = configuration;
            _captchaVerificationService = captchaVerificationService;
        }

        // GET: complaints/property-types
        [HttpGet]
        [Route("property-types")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            _logger.LogInformation("Attempting to retrieve property types");
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

        // GET: complaints/provinces
        [HttpGet]
        [Route("provinces")]
        public IActionResult GetProvinces()
        {
            _logger.LogInformation("Attempting to retrieve provinces");
            try
            {
                var data = Province.GetProvinces();
                _logger.LogInformation("Successfully retrieved {@Provinces}", data);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve provinces");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: complaints/csa
        [HttpPost]
        [Route("csa")]
        [RequiresCSAEnabled]
        public async Task<IActionResult> PostCSA([FromBody] Complaint complaint, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Attempting to submit CSA complaint {@Complaint}", complaint);

                var isValidCaptcha = await _captchaVerificationService.VerifyAsync(complaint.Captcha, ct);
                if (!isValidCaptcha)
                {
                    _logger.LogWarning("Captcha verification failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                complaint.LegislationType = Constants.LegislationTypeCSA;
                complaint.Sanitize();

                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                await SubmitComplaintToDynamicsAsync(complaint);
                _logger.LogInformation("Added CSA complaint to dynamics");
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
        public async Task<IActionResult> PostCCLA([FromBody] Complaint complaint, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Attempting to submit CCLA complaint {@Complaint}", complaint, ct);

                var isValidCaptcha = await _captchaVerificationService.VerifyAsync(complaint.Captcha, ct);
                if (!isValidCaptcha)
                {
                    _logger.LogWarning("Captcha verification failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                complaint.LegislationType = Constants.LegislationTypeCCLA;
                complaint.Sanitize();

                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                await SubmitComplaintToDynamicsAsync(complaint);
                _logger.LogInformation("Added CCLA complaint to dynamics");
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
                _logger.LogInformation(
                    "Successfully created complaint {ComplaintNumber} from view model {@Complaint}",
                    result.CsuName,
                    complaint
                );
            }
            catch (OdataerrorException ex)
            {
                _logger.LogError(
                    ex,
                    string.Join(Environment.NewLine, "Failed to create complaint", "{@ErrorBody}"),
                    ex.Body
                );
                throw;
            }
        }
    }
}
