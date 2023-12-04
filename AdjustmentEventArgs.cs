using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class AdjustmentEventArgs : EventArgs
    {
        public WHitem OriginalItem { get; }
        public WHitem AdjustedItemA { get; }
        public WHitem AdjustedItemB { get; }
        public AdjustmentEventArgs(WHitem originalItem, WHitem adjustedItemA, WHitem adjustedItemB)
        {
            OriginalItem = originalItem;
            AdjustedItemA = adjustedItemA;
            AdjustedItemB = adjustedItemB;
        }
    }
}
