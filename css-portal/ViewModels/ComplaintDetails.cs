using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class ComplaintDetails
    {
        public string Name { get; set; }

        public Address Address { get; set; }

        public string PropertyType { get; set; }

        public string OtherPropertyType { get; set; }

        public string Description { get; set; }

        public string Problems { get; set; }

        public string OccupantName { get; set; }

        public string OwnerName { get; set; }

        public async Task<bool> Validate(ComplaintType type)
        {
            var propertyTypes = await ViewModels.PropertyType.GetPropertyTypesAsync();
            if (!string.IsNullOrEmpty(PropertyType) && propertyTypes.All(a => a.Value != PropertyType))
            {
                return false;
            }

            if (Address == null || string.IsNullOrWhiteSpace(Address.City))
            {
                return false;
            }

            if (type == ComplaintType.CSA && string.IsNullOrWhiteSpace(Description))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Problems))
            {
                return false;
            }

            return true;
        }
    }
}
