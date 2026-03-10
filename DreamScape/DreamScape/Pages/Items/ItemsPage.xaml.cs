using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamScape.Pages.Items
{
    public sealed partial class ItemsPage : Page
    {
        private List<Item> _allItems = new();
        private bool _loaded = false;

        public ItemsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            using var db = new AppDbContext();
            _allItems = await db.Items.ToListAsync();
            _loaded = true;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (!_loaded) return;

            string type = (TypeFilter.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";
            string rarity = (RarityFilter.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";

            IEnumerable<Item> filtered = _allItems;

            if (type != "All")
                filtered = filtered.Where(i => i.Type == type);

            if (rarity != "All")
                filtered = filtered.Where(i => i.Rarity == rarity);

            ItemsListView.ItemsSource = filtered.ToList();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
