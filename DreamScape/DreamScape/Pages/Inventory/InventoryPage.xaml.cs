using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace DreamScape.Pages.Inventory
{
    public sealed partial class InventoryPage : Page
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadInventoryAsync();
        }

        private async Task LoadInventoryAsync()
        {
            if (AppSession.CurrentUser == null) return;

            HeaderTextBlock.Text = $"{AppSession.CurrentUser.Username}'s Inventory";

            using var db = new AppDbContext();
            var items = await db.Inventories
                .Include(inv => inv.Item)
                .Where(inv => inv.UserId == AppSession.CurrentUser.UserId)
                .ToListAsync();

            InventoryListView.ItemsSource = items;
        }
    }
}
