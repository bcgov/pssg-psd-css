using Gov.Pssg.Css.Public.ViewModels;
using Microsoft.Extensions.Configuration;
using System;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class StatusUtility
    {
        public static Status GetStatus(IConfiguration configuration)
        {
            return new Status
            {
                CaptchaApiUrl = configuration["CAPTCHA_API_URL"],
                CsaEnabled = IsCSAEnabled(configuration),
                UnderMaintenance = IsUnderMaintenance(configuration),
            };
        }

        public static bool IsCSAEnabled(IConfiguration configuration)
        {
            return string.Equals(configuration["CSA_ENABLED"], "true", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsUnderMaintenance(IConfiguration configuration)
        {
            return string.Equals(configuration["UNDER_MAINTENANCE"], "true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
