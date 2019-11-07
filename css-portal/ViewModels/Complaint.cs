using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Complaint
    {
        public ComplaintDetails Details { get; set; }

        public Complainant Complainant { get; set; }

        public async Task<bool> Validate(ComplaintType type)
        {
            if (Details == null)
            {
                return false;
            }

            bool valid = true;

            valid &= await Details.Validate(type);

            if (Complainant != null)
            {
                bool complainantValid = await Complainant.Validate(type);
                if (!complainantValid)
                {
                    Complainant = null;
                }
            }

            if (Complainant == null && type == ComplaintType.CSA)
            {
                valid = false;
            }

            return valid;
        }
    }
}
