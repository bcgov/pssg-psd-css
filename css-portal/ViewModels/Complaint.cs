using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Complaint
    {
        public Property Property { get; set; }

        public Complainant Complainant { get; set; }

        public async Task<bool> Validate()
        {
            if (Property == null)
            {
                return false;
            }

            bool valid = true;

            valid &= await Property.Validate();

            if (Complainant != null)
            {
                valid &= await Complainant.Validate();
            }

            return valid;
        }
    }
}
