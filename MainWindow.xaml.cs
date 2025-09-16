using System;
using System.Windows;

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
                // Ouvrir la fenêtre de gestion des utilisateurs
                // Correction : Il ne faut pas ouvrir une nouvelle instance de MainWindow ici,
                // car cela crée une boucle d'ouverture de la même fenêtre.
                // Utilisez la fenêtre appropriée, par exemple UtilisateursWindow.
                // Exemple :
                // UtilisateursWindow utilisateursWindow = new UtilisateursWindow();
                // utilisateursWindow.Show();

                MessageBox.Show("Fonctionnalité de gestion des utilisateurs à implémenter",
                                "Gestion Utilisateurs", MessageBoxButton.OK, MessageBoxImage.Information);

                // Optionnel: fermer la page d'accueil
                // this.Close();
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
                // Ici vous pouvez créer une nouvelle fenêtre pour la gestion des casiers
                MessageBox.Show("Fonctionnalité de gestion des casiers à implémenter",
                              "Gestion Casiers", MessageBoxButton.OK, MessageBoxImage.Information);

                // Exemple pour une future fenêtre:
                // CasiersWindow casiersWindow = new CasiersWindow();
                // casiersWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la gestion casiers: {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            // Ajoutez ici la logique pour ouvrir le panneau d'administration
            MessageBox.Show("Ouverture du panneau d'administration.");
        }
    }
}
