using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class KitHistoryItem
    {
        public string? DateOfCreation { get; set; }
        public string? ProjectName { get; set; }
        public string? IPN { get; set; }
        public string? MFPN { get; set; }
        public string? Description { get; set; }
        public int? QtyInKit { get; set; }
        public int? Delta { get; set; }
        public int? QtyPerUnit { get; set; }
        public string? Calc { get; set; }
        public string? Alts { get; set; }
        public string? filePath { get; set; }
    }
}
