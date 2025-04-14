using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class WarehouseBalance
    {
        public string LOCNAME { get; set; }
        public string PARTNAME { get; set; }
        public string PARTDES { get; set; }
        public string SERIALNAME { get; set; }
        public string SERIALDES { get; set; }
        public string EXPIRYDATE { get; set; }
        public string DOCNO { get; set; }
        public string PROJDES { get; set; }
        public string ACTNAME { get; set; }
        public string CUSTNAME { get; set; }
        public int TBALANCE { get; set; }
        public string TUNITNAME { get; set; }
        public string SUPNAME { get; set; }
        public string SUPDES { get; set; }
        public int BALANCE { get; set; }
        public string UNITNAME { get; set; }
        public string CDATE { get; set; }
        public int NUMPACK { get; set; }
        public int WARHS { get; set; }
        public int PART { get; set; }
        public int CUST { get; set; }
        public int ACT { get; set; }
        public int SERIAL { get; set; }
        public string MNFPARTNAME { get; set; } // Add this property
    }
}
