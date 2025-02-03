using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH_Panel
{
    public class Warehouse
    {
        public string WARHSNAME { get; set; }
        public string WARHSDES { get; set; }
        public int WARHS { get; set; }
        public List<WarehouseBalance> WARHSBAL_SUBFORM { get; set; }
    }
}
