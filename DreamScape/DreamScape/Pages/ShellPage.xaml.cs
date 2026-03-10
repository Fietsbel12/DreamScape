using DreamScape.Pages.Items;
using DreamScape.Pages.Inventory;
using DreamScape.Pages.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace DreamScape.Pages
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (AppSession.IsAdmin)
            {
                AdminItemsNavItem.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                AdminPlayersNavItem.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }

            // Select and navigate to Items by default
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(ItemsPage));
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                string tag = item.Tag?.ToString() ?? string.Empty;

                Type? pageType = tag switch
                {
                    "ItemsPage" => typeof(ItemsPage),
                    "InventoryPage" => typeof(InventoryPage),
                    "TradeService" => typeof(TradeService),
                    _ => null
                };

                if (tag == "Logout")
                {
                    AppSession.CurrentUser = null;
                    Frame.Navigate(typeof(WelcomePage));
                    return;
                }

                if (pageType != null)
                    ContentFrame.Navigate(pageType);
            }
        }
    }
}
