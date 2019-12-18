using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Gov.Pssg.Css.Public.Utility;

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

        public void Sanitize()
        {
            if (Country == "Canada")
            {
                ZipPostalCode = ZipPostalCode?.ToUpperInvariant();
                if (ZipPostalCode?.Length == 6)
                {
                    ZipPostalCode = string.Join(" ", ZipPostalCode.Substring(0, 3), ZipPostalCode.Substring(3, 3));
                }
            }
        }

        public bool ValidateForProperty()
        {
            if (string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(ProvinceState) ||
                string.IsNullOrWhiteSpace(Country))
            {
                return false;
            }

            if (!ValidateZipPostalCode())
            {
                return false;
            }

            return true;
        }

        public bool ValidateForComplainant(int legislationType)
        {
            if (legislationType == Constants.LegislationTypeCSA &&
                (string.IsNullOrWhiteSpace(Line1) ||
                 string.IsNullOrWhiteSpace(City) ||
                 string.IsNullOrWhiteSpace(ProvinceState) ||
                 string.IsNullOrWhiteSpace(Country)))
            {
                return false;
            }

            if (!ValidateZipPostalCode())
            {
                return false;
            }

            return true;
        }

        private bool ValidateZipPostalCode()
        {
            return Country != "Canada" ||
                   string.IsNullOrWhiteSpace(ZipPostalCode) ||
                   Regex.Match(ZipPostalCode, @"^[A-Z]\d[A-Z] \d[A-Z]\d$").Success;
        }
    }
}
