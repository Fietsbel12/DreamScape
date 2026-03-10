using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Services
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TradeService : Page
    {
        public TradeService()
        {
            InitializeComponent();

            int currentUserId = 1;
            LoadReceivedTrades(currentUserId); // LATER AANPASSEN WNR LOGIN IS GEMAAKT!!! AUB FOR THE LOVE OF GOD
        }

        private void LoadReceivedTrades(int UserId)
        {
            using var db = new AppDbContext();
            
            TradesListView.ItemsSource = db.Trades
                .Include(t => t.Sender)
                .Where(t => t.ReceiverId == UserId && t.Status == TradeStatus.Pending)
                .OrderByDescending(t => t.TradeId)
                .ToList();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTrade = TradesListView.SelectedItem as Trade;

            if (selectedTrade == null)
                return;

            using var db = new AppDbContext();

            var trade = db.Trades.FirstOrDefault(t => t.TradeId == selectedTrade.TradeId);
            if (trade != null)
            {
                trade.Status = TradeStatus.Accepted;
                db.SaveChanges();
            }

            LoadReceivedTrades(selectedTrade.ReceiverId);
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTrade = TradesListView.SelectedItem as Trade;

            if (selectedTrade == null)
                return;

            using var db = new AppDbContext();

            var trade = db.Trades.FirstOrDefault(t => t.TradeId == selectedTrade.TradeId);
            if (trade != null)
            {
                trade.Status = TradeStatus.Declined;
                db.SaveChanges();
            }

            LoadReceivedTrades(selectedTrade.ReceiverId);
        }
    }
}
