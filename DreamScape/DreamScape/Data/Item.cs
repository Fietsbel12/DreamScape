using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    internal class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }

        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Durability { get; set; }
        public string MagicalProperty { get; set; }

        public ICollection<InventoryItem> InventoryItems { get; set; }
        public ICollection<TradeItem> TradeItems { get; set; }
    }
}
