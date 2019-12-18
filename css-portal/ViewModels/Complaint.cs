using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Complaint
    {
        public int? LegislationType { get; set; }

        public ComplaintDetails Details { get; set; }

        public Complainant Complainant { get; set; }

        public string AuthorizationToken { get; set; }

        public void Sanitize()
        {
            Details?.Sanitize();
            Complainant?.Sanitize();
        }

        public async Task<bool> Validate()
        {
            if (LegislationType != Constants.LegislationTypeCCLA && LegislationType != Constants.LegislationTypeCSA)
            {
                return false;
            }

            if (Details == null)
            {
                return false;
            }

            if (!await Details.Validate(LegislationType.Value))
            {
                return false;
            }

            if (Complainant != null && !Complainant.Validate(LegislationType.Value))
            {
                return false;
            }

            if (Complainant == null && LegislationType == Constants.LegislationTypeCSA)
            {
                return false;
            }

            return true;
        }
    }
}
