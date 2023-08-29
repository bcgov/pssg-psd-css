using Gov.Pssg.Css.Public.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Pssg.Css.Public.Attributes
{
    public class UnavailableDuringMaintenanceAttribute : TypeFilterAttribute
    {
        public UnavailableDuringMaintenanceAttribute() : base(typeof(UnavailableDuringMaintenanceFilter)) { }
    }
}
