using System;
using System.Collections.Generic;
using System.Linq;
using Gov.Pssg.Css.Public.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Gov.Pssg.Css.Public.Filters
{
    public class UnavailableDuringMaintenanceFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public UnavailableDuringMaintenanceFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (StatusUtility.IsUnderMaintenance(_configuration))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
