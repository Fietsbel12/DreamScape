using DreamScape.Data;
using DreamScape.Pages.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Account
{
    /// <summary>
    /// Login page for DreamScape employees.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            await AttemptLogin();
        }

        private async Task AttemptLogin()
        {
            string naam = NaamTextBox.Text.Trim();
            string wachtwoord = WachtwoordBox.Password;

            if (string.IsNullOrEmpty(naam) || string.IsNullOrEmpty(wachtwoord))
            {
                await ShowDialog("Login mislukt", "Vul zowel naam als wachtwoord in.");
                return;
            }

            using var db = new AppDbContext();

            var gebruiker = db.Users
                              .Include(u => u.Role)
                              .FirstOrDefault(u => u.Username.ToLower() == naam.ToLower());

            if (gebruiker != null && BCrypt.Net.BCrypt.Verify(wachtwoord, gebruiker.PasswordHash))
            {
                Frame.Navigate(typeof(MedewerkerDashboard), gebruiker.Role?.Name ?? "Player");
            }
            else
            {
                await ShowDialog("Login mislukt", "Naam of wachtwoord is onjuist.");
                WachtwoordBox.Password = string.Empty;
                WachtwoordBox.Focus(FocusState.Programmatic);
            }
        }

        private async Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = title,
                Content = content,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void OwnerLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MedewerkerDashboard), "Eigenaar");
        }

        private async void WachtwoordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                await AttemptLogin();
            }
        }
    }
}
