using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Address
    {
        public string Unit { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Country { get; set; }

        public string ProvinceState { get; set; }

        public string City { get; set; }

        public string ZipPostalCode { get; set; }

        public async Task<bool> Validate()
        {
            if (City == null || ProvinceState == null || Country == null)
            {
                return false;
            }

            return true;
        }
    }
}
