using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class StatusUtility
    {
        public static bool IsUnderMaintenance(IConfiguration configuration)
        {
            return string.Equals(configuration["UNDER_MAINTENANCE"], "true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
