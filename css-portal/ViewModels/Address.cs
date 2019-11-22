using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Address
    {
        [StringLength(250)]
        public string Unit { get; set; }

        [StringLength(250)]
        public string Line1 { get; set; }

        [StringLength(80)]
        public string Country { get; set; }

        [StringLength(50)]
        public string ProvinceState { get; set; }

        [StringLength(80)]
        public string City { get; set; }

        [StringLength(20)]
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
