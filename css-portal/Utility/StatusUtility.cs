using System;
using Gov.Pssg.Css.Public.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class StatusUtility
    {
        public static Status GetStatus(IConfiguration configuration)
        {
            return new Status
            {
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
            return string.Equals(
                configuration["UNDER_MAINTENANCE"],
                "true",
                StringComparison.InvariantCultureIgnoreCase
            );
        }
    }
}
