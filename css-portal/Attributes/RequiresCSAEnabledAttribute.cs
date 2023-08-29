using Gov.Pssg.Css.Public.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Pssg.Css.Public.Attributes
{
    public class RequiresCSAEnabledAttribute : TypeFilterAttribute
    {
        public RequiresCSAEnabledAttribute() : base(typeof(RequiresCSAEnabledFilter)) { }
    }
}
