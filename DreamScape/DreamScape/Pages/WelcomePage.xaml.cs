using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace DreamScape.Pages
{
    public sealed partial class WelcomePage : Page
    {
        // Dummy hash used when a user is not found, to keep verification time constant
        // and prevent username enumeration via timing attacks.
        private static readonly string _dummyHash = BCrypt.Net.BCrypt.HashPassword("dummy");

        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await ShowErrorAsync("Please enter a username and password.");
                return;
            }

            using var db = new AppDbContext();
            var user = await db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            string hashToVerify = user?.PasswordHash ?? _dummyHash;
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, hashToVerify);

            if (user == null || !passwordValid)
            {
                await ShowErrorAsync("Invalid username or password.");
                return;
            }

            AppSession.CurrentUser = user;
            Frame.Navigate(typeof(ShellPage));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
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
    }
}

