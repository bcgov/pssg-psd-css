using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.ViewModels
{
    public class Status
    {
        public string CaptchaApiUrl { get; set; }

        public bool CsaEnabled { get; set; }

        public bool UnderMaintenance { get; set; }
    }
}
