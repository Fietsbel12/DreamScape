using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    public enum TradeStatus
    {
        Pending,
        Accepted,
        Declined
    }
    internal class Trade
    {
        public int TradeId { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public TradeStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TradeItem> TradeItems { get; set; }
    }
}
