using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    internal class InventoryItem
    {
        public int InventoryItemId { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}
