using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class StatusUtility
    {
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
