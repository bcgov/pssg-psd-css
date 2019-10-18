using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Form
    {
        public string Activity { get; set; }

        public string Location { get; set; }

        public string Details { get; set; }

        public DateTime? Date { get; set; }

        public async Task<bool> Validate()
        {
            var activities = await GetActivitiesAsync();
            if (Activity == null || activities.All(a => a.Value != Activity))
            {
                return false;
            }

            if (Location == null)
            {
                return false;
            }

            return true;
        }

        public static async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            var activities = new List<Activity>
            {
                new Activity("Drug Production", "drug_production"),
                new Activity("Drug Trafficking", "drug_production"),
                new Activity("Illegal Firearm Possession", "firearm_possession"),
                new Activity("Other", "other"),
            };

            return activities;
        }
    }
}
