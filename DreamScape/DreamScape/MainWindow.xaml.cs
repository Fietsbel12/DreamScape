using DreamScape.Data;
using DreamScape.Pages;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape
{
    /// <summary>
    /// The main application window. Hosts the root navigation Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using var db = new AppDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            RootFrame.Navigate(typeof(WelcomePage));
        }
    }
}
