using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Admin
{
    /// <summary>
    /// Dashboard page shown after a successful login.
    /// </summary>
    public sealed partial class MedewerkerDashboard : Page
    {
        public MedewerkerDashboard()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string rol)
            {
                RolTextBlock.Text = $"Je bent ingelogd als: {rol}";
                DashboardTitle.Text = $"Dashboard – {rol}";
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
