using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Complainant
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string GovernmentAgency { get; set; }

        public Address Address { get; set; }

        public void Sanitize()
        {
            Phone = PhoneNumberUtility.Sanitize(Phone);
            Fax = PhoneNumberUtility.Sanitize(Fax);
        }

        public bool Validate(int legislationType)
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone) && string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            if (!PhoneNumberUtility.Validate(Phone) || !PhoneNumberUtility.Validate(Fax))
            {
                return false;
            }

            if (Address == null)
            {
                return false;
            }

            if (legislationType == Constants.LegislationTypeCSA)
            {
                if (string.IsNullOrWhiteSpace(Address.Line1) ||
                    string.IsNullOrWhiteSpace(Address.City) ||
                    string.IsNullOrWhiteSpace(Address.ProvinceState) ||
                    string.IsNullOrWhiteSpace(Address.Country))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
