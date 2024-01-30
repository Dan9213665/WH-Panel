using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH_Panel
{
    public class SIMIPNTABLE
    {
        public string IPN { get; set; }
        public decimal WHqty { get; set; }
        public int? KITsBalance  { get; set; }
        public decimal? DELTA { get; set;}

        public List<BOMitem>BOMITEMS { get; set; }
    }
}
