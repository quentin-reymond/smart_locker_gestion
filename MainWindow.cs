using System;
using System.Windows;

namespace gestion
{
    public partial class MainWindow : Window
    {
        // Supprimez tout constructeur supplémentaire MainWindow() dans ce fichier.
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGestionUtilisateurs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Fonctionnalité de gestion des utilisateurs à implémenter",
                                "Gestion Utilisateurs", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Fonctionnalité de gestion des casiers à implémenter",
                              "Gestion Casiers", MessageBoxButton.OK, MessageBoxImage.Information);
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