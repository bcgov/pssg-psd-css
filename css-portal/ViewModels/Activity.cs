using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Activity
    {
        public string Name { get; }

        public string Value { get; }

        public Activity(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
