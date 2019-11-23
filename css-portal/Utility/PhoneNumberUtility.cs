using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class PhoneNumberUtility
    {
        public static string Sanitize(string number)
        {
            var digits = number?.Where(char.IsDigit).ToArray();
            if (digits == null || digits.Length == 0)
            {
                return null;
            }

            return new string(digits);
        }

        public static bool Validate(string number)
        {
            return number == null || number.Length == 10;
        }
    }
}
