using System;
using System.Windows;
using System.Windows.Controls;

namespace gestion
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGestionUtilisateurs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                user gestionUtilisateursWindow = new user(); // Crée une nouvelle instance de la fenêtre de gestion des utilisateurs
                gestionUtilisateursWindow.Show(); // Affiche la fenêtre
                this.Close(); // Ferme la fenêtre principale
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la gestion utilisateurs: {ex.Message}",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGestionCasiers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                locker gestionCasiersPage = new locker(); // Crée une nouvelle instance de la page de gestion des casiers
                Frame frame = new Frame(); // Crée un nouveau Frame pour afficher la page
                frame.Navigate(gestionCasiersPage); // Navigue vers la page de gestion des casiers
                this.Content = frame; // Remplace le contenu de la fenêtre actuelle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la gestion casiers: {ex.Message}",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panneau d'administration.");
        }
    }
}