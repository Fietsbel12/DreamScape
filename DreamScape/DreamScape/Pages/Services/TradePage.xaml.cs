using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Services
{
    /// <summary>
    /// Dashboard page showing the logged-in player's inventory and pending trades.
    /// </summary>
    public sealed partial class TradeService : Page
    {
        private int _currentUserId;

        public TradeService()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int userId)
            {
                _currentUserId = userId;
                LoadDashboard();
            }
            else
            {
                Frame.Navigate(typeof(DreamScape.Pages.Account.WelcomePage));
                Frame.BackStack.Clear();
            }
        }

        private void LoadDashboard()
        {
            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(u => u.UserId == _currentUserId);
            if (user != null)
            {
                WelcomeText.Text = $"Welcome, {user.Username}!";
            }

            LoadReceivedTrades();
            LoadInventory();
        }

        private void LoadReceivedTrades()
        {
            using var db = new AppDbContext();

            TradesListView.ItemsSource = db.Trades
                .Include(t => t.Sender)
                .Where(t => t.ReceiverId == _currentUserId && t.Status == TradeStatus.Pending)
                .OrderByDescending(t => t.TradeId)
                .ToList();
        }

        private void LoadInventory()
        {
            using var db = new AppDbContext();

            var inventory = db.Inventories
                .Include(i => i.InventoryItems)
                .ThenInclude(ii => ii.Item)
                .FirstOrDefault(i => i.UserId == _currentUserId);

            InventoryListView.ItemsSource = inventory?.InventoryItems?.ToList()
                ?? new List<InventoryItem>();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int tradeId)
            {
                UpdateTradeStatus(tradeId, TradeStatus.Accepted);
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int tradeId)
            {
                UpdateTradeStatus(tradeId, TradeStatus.Declined);
            }
        }

        private void UpdateTradeStatus(int tradeId, TradeStatus status)
        {
            using var db = new AppDbContext();

            var trade = db.Trades.FirstOrDefault(t => t.TradeId == tradeId);
            if (trade != null)
            {
                trade.Status = status;
                db.SaveChanges();
            }

            LoadReceivedTrades();
        }

        private void CreateTradeButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to create trade page
        }
    }
}
