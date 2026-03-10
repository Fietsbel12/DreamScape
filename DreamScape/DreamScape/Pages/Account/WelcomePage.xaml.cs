using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Account
{
    /// <summary>
    /// Welcome page shown on app startup with login and create account options.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to login page (to be implemented)
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateAccount));
            Frame.BackStack.Clear();
        }
    }
}
