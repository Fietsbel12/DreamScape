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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WelcomePage));
            Frame.BackStack.Clear();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorsTextblock.Text = "";
            SuccessTextblock.Text = "";

            // USERNAME VALIDATION
            string username = UsernameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ErrorsTextblock.Text = "Username is required.";
                return;
            }
            if (username.Length > 50)
            {
                ErrorsTextblock.Text = "Invalid username.";
                return;
            }
            if (username.Length < 2)
            {
                ErrorsTextblock.Text = "Username must be at least 2 characters.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                ErrorsTextblock.Text = "Username may only contain letters, numbers and underscores.";
                return;
            }

            // PASSWORD VALIDATION
            string password = PasswordInput.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorsTextblock.Text = "Password is required.";
                return;
            }
            if (password.Length > 50)
            {
                ErrorsTextblock.Text = "Invalid password.";
                return;
            }
            if (password.Length < 6)
            {
                ErrorsTextblock.Text = "Password must be at least 6 characters.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]"))
            {
                ErrorsTextblock.Text = "Password must contain at least one digit.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
            {
                ErrorsTextblock.Text = "Password must contain at least one uppercase letter.";
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[\W_]"))
            {
                ErrorsTextblock.Text = "Password must contain at least one special character.";
                return;
            }

            // HASH THE PASSWORD
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // CREATE OBJECT AFTER VALIDATIONS
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                RoleId = 1
            };

            try
            {
                using var db = new AppDbContext();

                if (db.Users.Any(u => u.Username == username))
                {
                    ErrorsTextblock.Text = "Username is already taken.";
                    return;
                }

                db.Users.Add(user);
                db.SaveChanges();

                SuccessTextblock.Text = "Account successfully created!";

                await Task.Delay(1000);

                Frame.Navigate(typeof(WelcomePage));
                Frame.BackStack.Clear();
            }
            catch (Exception ex)
            {
                ErrorsTextblock.Text = $"Error while saving: {ex.Message}";
            }
        }
    }
}
