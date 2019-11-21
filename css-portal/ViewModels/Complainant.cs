using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Complainant
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string GovernmentAgency { get; set; }

        public Address Address { get; set; }

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
