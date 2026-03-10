using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamScape.Pages.Services
{
    public sealed partial class TradeService : Page
    {
        public TradeService()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadTradesAsync();
            await LoadSendFormAsync();
        }

        private async Task LoadTradesAsync()
        {
            if (AppSession.CurrentUser == null) return;

            using var db = new AppDbContext();
            var trades = await db.Trades
                .Include(t => t.Sender)
                .Include(t => t.TradeItems)
                    .ThenInclude(ti => ti.Item)
                .Where(t => t.ReceiverId == AppSession.CurrentUser.UserId && t.Status == "Pending")
                .ToListAsync();

            TradeListView.ItemsSource = trades.Select(t => new TradeViewModel
            {
                TradeId = t.TradeId,
                Sender = t.Sender,
                OfferedItemsSummary = string.Join(", ",
                    t.TradeItems.Select(ti => $"{ti.Item.Name} x{ti.Quantity}"))
            }).ToList();
        }

        private async Task LoadSendFormAsync()
        {
            if (AppSession.CurrentUser == null) return;

            using var db = new AppDbContext();

            var players = await db.Users
                .Where(u => u.UserId != AppSession.CurrentUser.UserId)
                .ToListAsync();
            ReceiverComboBox.ItemsSource = players;
            ReceiverComboBox.DisplayMemberPath = "Username";

            var inventory = await db.Inventories
                .Include(inv => inv.Item)
                .Where(inv => inv.UserId == AppSession.CurrentUser.UserId)
                .ToListAsync();
            ItemComboBox.ItemsSource = inventory;
            ItemComboBox.DisplayMemberPath = "Item.Name";
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int tradeId)
                await UpdateTradeStatusAsync(tradeId, "Accepted");
        }

        private async void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int tradeId)
                await UpdateTradeStatusAsync(tradeId, "Declined");
        }

        private async Task UpdateTradeStatusAsync(int tradeId, string status)
        {
            using var db = new AppDbContext();
            var trade = await db.Trades
                .Include(t => t.TradeItems)
                .FirstOrDefaultAsync(t => t.TradeId == tradeId);
            if (trade == null) return;

            trade.Status = status;

            if (status == "Accepted")
            {
                foreach (var tradeItem in trade.TradeItems)
                {
                    // Remove from sender's inventory
                    var senderInv = await db.Inventories
                        .FirstOrDefaultAsync(inv => inv.UserId == trade.SenderId && inv.ItemId == tradeItem.ItemId);
                    if (senderInv != null)
                    {
                        senderInv.Quantity -= tradeItem.Quantity;
                        if (senderInv.Quantity <= 0)
                            db.Inventories.Remove(senderInv);
                    }

                    // Add to receiver's inventory
                    var receiverInv = await db.Inventories
                        .FirstOrDefaultAsync(inv => inv.UserId == trade.ReceiverId && inv.ItemId == tradeItem.ItemId);
                    if (receiverInv != null)
                        receiverInv.Quantity += tradeItem.Quantity;
                    else
                        db.Inventories.Add(new Data.Inventory
                        {
                            UserId = trade.ReceiverId,
                            ItemId = tradeItem.ItemId,
                            Quantity = tradeItem.Quantity
                        });
                }
            }

            await db.SaveChangesAsync();
            await LoadTradesAsync();
        }

        private async void SendTradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiverComboBox.SelectedItem is not User receiver)
            {
                await ShowErrorAsync("Please select a player to trade with.");
                return;
            }

            if (ItemComboBox.SelectedItem is not Data.Inventory selectedInventory)
            {
                await ShowErrorAsync("Please select an item to offer.");
                return;
            }

            using var db = new AppDbContext();

            // Verify sender still owns the item
            var senderInventory = await db.Inventories
                .FirstOrDefaultAsync(inv => inv.UserId == AppSession.CurrentUser!.UserId
                                         && inv.ItemId == selectedInventory.ItemId
                                         && inv.Quantity >= 1);
            if (senderInventory == null)
            {
                await ShowErrorAsync("You no longer own this item.");
                await LoadSendFormAsync();
                return;
            }

            var trade = new Trade
            {
                SenderId = AppSession.CurrentUser!.UserId,
                ReceiverId = receiver.UserId,
                Status = "Pending"
            };
            db.Trades.Add(trade);
            await db.SaveChangesAsync();

            db.TradeItems.Add(new TradeItem
            {
                TradeId = trade.TradeId,
                ItemId = selectedInventory.ItemId,
                Quantity = 1
            });
            await db.SaveChangesAsync();

            ReceiverComboBox.SelectedIndex = -1;
            ItemComboBox.SelectedIndex = -1;

            await ShowInfoAsync("Trade request sent!");
        }

        private async Task ShowErrorAsync(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task ShowInfoAsync(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = XamlRoot
            };
            await dialog.ShowAsync();
        }

        private class TradeViewModel
        {
            public int TradeId { get; set; }
            public User Sender { get; set; } = null!;
            public string OfferedItemsSummary { get; set; } = string.Empty;
        }
    }
}

