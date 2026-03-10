using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Account
{
    /// <summary>
    /// Page that allows any user to create a new account.
    /// </summary>
    public sealed partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WelcomePage));
            Frame.BackStack.Clear();
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorsTextblock.Text = "";
            SuccessTextblock.Text = "";

            // USERNAME VALIDATIE
            string username = UsernameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ErrorsTextblock.Text = "Gebruikersnaam is verplicht.";
                return;
            }
            if (username.Length > 50)
            {
                ErrorsTextblock.Text = "Ongeldige gebruikersnaam.";
                return;
            }
            if (username.Length < 2)
            {
                ErrorsTextblock.Text = "Gebruikersnaam moet minimaal 2 tekens bevatten.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                ErrorsTextblock.Text = "Gebruikersnaam mag alleen letters, cijfers en underscores bevatten.";
                return;
            }

            // WACHTWOORD VALIDATIE
            string wachtwoord = PasswordTextBox.Password;

            if (string.IsNullOrWhiteSpace(wachtwoord))
            {
                ErrorsTextblock.Text = "Wachtwoord is verplicht.";
                return;
            }
            if (wachtwoord.Length > 50)
            {
                ErrorsTextblock.Text = "Ongeldig wachtwoord.";
                return;
            }
            if (wachtwoord.Length < 6)
            {
                ErrorsTextblock.Text = "Wachtwoord moet minimaal 6 tekens bevatten.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(wachtwoord, @"[0-9]"))
            {
                ErrorsTextblock.Text = "Wachtwoord moet minimaal één cijfer bevatten.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(wachtwoord, @"[A-Z]"))
            {
                ErrorsTextblock.Text = "Wachtwoord moet minimaal één hoofdletter bevatten.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(wachtwoord, @"[\W_]"))
            {
                ErrorsTextblock.Text = "Wachtwoord moet minimaal één speciaal teken bevatten.";
                return;
            }

            // HASH HET WACHTWOORD
            string gehashtWachtwoord = BCrypt.Net.BCrypt.HashPassword(wachtwoord);

            // AANGEMAAKT OBJECT NA VALIDATIES
            var user = new User
            {
                Username = username,
                PasswordHash = gehashtWachtwoord,
                RoleId = 1 // Player role
            };

            try
            {
                using var db = new AppDbContext();

                bool usernameExists = await db.Users.AnyAsync(u => u.Username == username);

                if (usernameExists)
                {
                    ErrorsTextblock.Text = "Gebruikersnaam is al in gebruik.";
                    return;
                }

                db.Users.Add(user);
                await db.SaveChangesAsync();

                SuccessTextblock.Text = "Account succesvol aangemaakt!";

                await Task.Delay(1000);

                Frame.Navigate(typeof(WelcomePage));
                Frame.BackStack.Clear();
            }
            catch (Exception ex)
            {
                ErrorsTextblock.Text = $"Fout bij opslaan: {ex.Message}";
            }
        }
    }
}
