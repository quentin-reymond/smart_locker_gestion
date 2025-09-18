using System.Windows;
using System.Windows.Controls;

namespace gestion
{
    public partial class locker : Page
    {
        public locker()
        {
            InitializeComponent();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Crée une nouvelle instance de la fenêtre principale
            mainWindow.Show(); // Affiche la fenêtre principale
            Window.GetWindow(this).Close(); // Ferme la fenêtre actuelle
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panel administrateur", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}