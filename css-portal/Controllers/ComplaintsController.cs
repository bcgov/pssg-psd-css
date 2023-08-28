using System;
using System.Threading.Tasks;
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

        private readonly string CaptchaNonce = "submit-complaint";

        public ComplaintsController(ILogger<ComplaintsController> logger, IDynamicsClient dynamicsClient, IConfiguration configuration)
        {
            _logger = logger;
            _dynamicsClient = dynamicsClient;
            _configuration = configuration;
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
        public async Task<IActionResult> PostCSA([FromBody] Complaint complaint)
        {
            _logger.LogInformation("Attempting to submit CSA complaint {@Complaint}", complaint);
            try
            {
                complaint.LegislationType = Constants.LegislationTypeCSA;
                complaint.Sanitize();

                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                bool authenticationResult = AuthenticateCaptchaToken(complaint.AuthorizationToken, _configuration["CAPTCHA_SECRET"]);
                if (authenticationResult == false)
                {
                    _logger.LogWarning("Captcha token authentication failed for complaint {@Complaint}", complaint);
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
        public async Task<IActionResult> PostCCLA([FromBody] Complaint complaint)
        {
            _logger.LogInformation("Attempting to submit CCLA complaint {@Complaint}", complaint);
            try
            {
                complaint.LegislationType = Constants.LegislationTypeCCLA;
                complaint.Sanitize();

                bool validationResult = await complaint.Validate();
                if (validationResult == false)
                {
                    _logger.LogWarning("Validation failed for complaint {@Complaint}", complaint);
                    return BadRequest();
                }

                bool authenticationResult = AuthenticateCaptchaToken(complaint.AuthorizationToken, _configuration["CAPTCHA_SECRET"]);
                if (authenticationResult == false)
                {
                    _logger.LogWarning("Captcha token authentication failed for complaint {@Complaint}", complaint);
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
                _logger.LogInformation("Successfully created complaint {ComplaintNumber} from view model {@Complaint}", result.CsuName, complaint);
            }
            catch (OdataerrorException ex)
            {
                _logger.LogError(ex, string.Join(Environment.NewLine, "Failed to create complaint", "{@ErrorBody}"), ex.Body);
                throw;
            }
        }

        private bool AuthenticateCaptchaToken(string token, string secret)
        {
            try
            {
                var payload = JwtUtility.TryDecode(token, secret, _logger);
                string nonce = payload.SelectToken("data.nonce")?.Value<string>();
                if (nonce == CaptchaNonce)
                {
                    return true;
                }

                _logger.LogWarning("Could not match expected captcha {Nonce} in {Payload}", CaptchaNonce, payload.ToString());
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
