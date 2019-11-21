using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Pssg.Css.Public.Utility;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class PropertyType
    {
        public string Name { get; }

        public int Value { get; }

        public PropertyType(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static readonly string LabelApartment = "Apartment";
        public static readonly string LabelCabin = "Cabin";
        public static readonly string LabelCommercial = "Commercial";
        public static readonly string LabelCondo = "Condo";
        public static readonly string LabelHouse = "House";
        public static readonly string LabelDuplex = "Duplex";
        public static readonly string LabelMobileHome = "Mobile Home";
        public static readonly string LabelTownhome = "Town-home";
        public static readonly string LabelOther = "Other";

        public static async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
        {
            var propertyTypes = new List<PropertyType>
            {
                new PropertyType(LabelApartment, Constants.PropertyTypeApartment),
                new PropertyType(LabelCabin, Constants.PropertyTypeCabin),
                new PropertyType(LabelCommercial, Constants.PropertyTypeCommercial),
                new PropertyType(LabelCondo, Constants.PropertyTypeCondo),
                new PropertyType(LabelHouse, Constants.PropertyTypeHouse),
                new PropertyType(LabelDuplex, Constants.PropertyTypeDuplex),
                new PropertyType(LabelMobileHome, Constants.PropertyTypeMobileHome),
                new PropertyType(LabelTownhome, Constants.PropertyTypeTownhome),
                new PropertyType(LabelOther, Constants.PropertyTypeOther),
            };

            return propertyTypes;
        }
    }
}
