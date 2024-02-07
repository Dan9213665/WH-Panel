using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class WHitem
    {
        public string? IPN { get; set; }
        public string? Manufacturer { get; set; }
        public string? MFPN { get; set; }
        public string? Description{ get; set; }
        public int Stock{ get; set; }
        public string? Updated_on { get; set; }
        public string? Comments{ get; set; }
        public string? Source_Requester { get; set; }
    }
}
