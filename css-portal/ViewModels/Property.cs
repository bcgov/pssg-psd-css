using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Property
    {
        public string Name { get; set; }

        public Address Address { get; set; }

        public string PropertyType { get; set; }

        public string OtherPropertyType { get; set; }

        public string Description { get; set; }

        public string Problems { get; set; }

        public string OccupantName { get; set; }

        public string OwnerName { get; set; }

        public async Task<bool> Validate()
        {
            var propertyTypes = await ViewModels.PropertyType.GetPropertyTypesAsync();
            if (!string.IsNullOrEmpty(PropertyType) && propertyTypes.All(a => a.Value != PropertyType))
            {
                return false;
            }

            return true;
        }
    }
}
