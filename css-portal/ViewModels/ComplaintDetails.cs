using Gov.Pssg.Css.Public.Utility;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class ComplaintDetails
    {
        [StringLength(250)]
        public string Name { get; set; }

        public Address Address { get; set; }

        public int? PropertyType { get; set; }

        [StringLength(100)]
        public string OtherPropertyType { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(2000)] // 10000 in Dynamics
        public string Problems { get; set; }

        [StringLength(150)]
        public string OccupantName { get; set; }

        [StringLength(100)]
        public string OwnerName { get; set; }

        public void Sanitize()
        {
            if (Address != null)
            {
                Address.ProvinceState = "British Columbia";
                Address.Country = "Canada";
                Address.Sanitize();
            }
        }

        public async Task<bool> Validate(int legislationType)
        {
            var propertyTypes = await ViewModels.PropertyType.GetPropertyTypesAsync();
            if (PropertyType != null && propertyTypes.All(a => a.Value != PropertyType))
            {
                return false;
            }

            if (Address == null || !Address.ValidateForProperty())
            {
                return false;
            }

            if (legislationType == Constants.LegislationTypeCSA && string.IsNullOrWhiteSpace(Description))
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
