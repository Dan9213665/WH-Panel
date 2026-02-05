using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH_Panel
{
    public class ReelState
    {
        // The Atomic Unique Key from Priority (TRANS)
        // Using long to match SQL BIGINT and handle large transaction IDs
        public long TransactionId { get; set; }
        public string DocNo { get; set; } // The Unique Key (GR25025556)
        public string PackageID { get; set; } // The Pack Code
        public int Qty { get; set; }
        public string User { get; set; }
        public DateTime? CountDate { get; set; }
        public bool IsCounted => CountDate.HasValue;

        // Metadata for Audit Trail
        public string BookNum { get; set; }
        public string Supplier { get; set; }
        public DateTime PriorityDate { get; set; }
    }
}
