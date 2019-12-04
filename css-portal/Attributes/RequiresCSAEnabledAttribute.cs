using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Pssg.Css.Public.Attributes
{
    public class RequiresCSAEnabledAttribute : TypeFilterAttribute
    {
        public RequiresCSAEnabledAttribute() : base(typeof(RequiresCSAEnabledFilter)) { }
    }
}
