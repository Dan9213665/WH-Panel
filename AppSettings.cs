using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class AppSettings
    {
        public string ApiUsername { get; set; }
        public string ApiPassword { get; set; }

        public string Api2Username { get; set; }
        public string Api2Password { get; set; }

        public string Api3Username { get; set; }
        public string Api3Password { get; set; }

        public string Api4Username { get; set; }
        public string Api4Password { get; set; }
        // Add other settings as needed
        public string MouserApiKey { get; set; } // <-- Add this line
    }
}
