using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages
{
    /// <summary>
    /// The welcome page shown when the application starts.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Replace <LoginPage> with the actual login page class when it is ready
            // Frame.Navigate(typeof(<LoginPage>));
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Replace <CreateAccountPage> with the actual create-account page class when it is ready
            // Frame.Navigate(typeof(<CreateAccountPage>));
        }
    }
}
