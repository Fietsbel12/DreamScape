using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace DreamScape.Pages
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await ShowErrorAsync("Please fill in all fields.");
                return;
            }

            if (password != confirmPassword)
            {
                await ShowErrorAsync("Passwords do not match.");
                return;
            }

            using var db = new AppDbContext();
            bool userExists = await db.Users.AnyAsync(u => u.Username == username);
            if (userExists)
            {
                await ShowErrorAsync("Username is already taken. Please choose another.");
                return;
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = 1 // Player
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            Frame.Navigate(typeof(WelcomePage));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WelcomePage));
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
