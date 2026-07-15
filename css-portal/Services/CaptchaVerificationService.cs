using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EMBC.Suppliers.API.Services
{
    public interface ICaptchaVerificationService
    {
        public Task<bool> VerifyAsync(string clientResponse, CancellationToken ct);
    }

    public class CaptchaVerificationServiceOptions
    {
        // Captcha secret key (server-side)
        public string Secret { get; set; } = null!;

        // Captcha verification URL
        public Uri Url { get; set; } = null!;
    }

    public class CaptchaResponse
    {
        public bool Success { get; set; }
    }

    /// <summary>
    /// Service for verifying CAPTCHA responses.
    /// </summary>
    public class CaptchaVerificationService : ICaptchaVerificationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly CaptchaVerificationServiceOptions options;
        private readonly ILogger<CaptchaVerificationService> _logger;

        public CaptchaVerificationService(
            IHttpClientFactory httpClientFactory,
            IOptions<CaptchaVerificationServiceOptions> options,
            ILogger<CaptchaVerificationService> logger
        )
        {
            this.httpClientFactory = httpClientFactory;
            this.options = options.Value;
            _logger = logger;
        }

        public async Task<bool> VerifyAsync(string clientResponse, CancellationToken ct)
        {
            var content = new Dictionary<string, string>()
            {
                { "secret", options.Secret },
                { "response", clientResponse },
            };

            using var client = httpClientFactory.CreateClient("captcha");

            var response = await client.PostAsync(
                options.Url.AbsoluteUri,
                new FormUrlEncodedContent(content),
                ct
            );

            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                "Captcha verification response: {ResponseBody}",
                await response.Content.ReadAsStringAsync(ct)
            );

            var responseData = await response.Content.ReadFromJsonAsync<CaptchaResponse>();

            return responseData.Success;
        }
    }
}
