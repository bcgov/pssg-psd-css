using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class PropertyType
    {
        public string Name { get; }

        public string Value { get; }

        public PropertyType(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
        {
            var propertyTypes = new List<PropertyType>
            {
                new PropertyType("Apartment", "apartment"),
                new PropertyType("Cabin", "cabin"),
                new PropertyType("Commercial", "commercial"),
                new PropertyType("Condo", "condo"),
                new PropertyType("House", "house"),
                new PropertyType("Duplex", "duplex"),
                new PropertyType("Mobile Home", "mobile_home"),
                new PropertyType("Townhome", "townhome"),
                new PropertyType("Other", "other"),
            };

            return propertyTypes;
        }
    }
}
