using DreamScape.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WelcomePage));
            Frame.BackStack.Clear();
        }

        private async void OpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorsTextblock.Text = "";
            SuccessTextblock.Text = "";

            // NAAM VALIDATIE
            string naam = NaamTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(naam))
            {
                ErrorsTextblock.Text = "Naam is verplicht.";
                return;
            }
            if (naam.Length > 50)
            {
                ErrorsTextblock.Text = "Ongeldige naam.";
                return;
            }
            if (naam.Length < 2)
            {
                ErrorsTextblock.Text = "Naam moet minimaal 2 tekens bevatten.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(naam, @"^[a-zA-Z0-9_]+$"))
            {
                ErrorsTextblock.Text = "Naam mag alleen letters, cijfers en underscores bevatten.";
                return;
            }

            // WACHTWOORD VALIDATIE
            string wachtwoord = WachtwoordPasswordBox.Password;

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
                Username = naam,
                PasswordHash = gehashtWachtwoord,
                RoleId = 1
            };

            try
            {
                using var db = new AppDbContext();

                if (db.Users.Any(u => u.Username == naam))
                {
                    ErrorsTextblock.Text = "Naam is al in gebruik.";
                    return;
                }

                db.Users.Add(user);
                db.SaveChanges();

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
