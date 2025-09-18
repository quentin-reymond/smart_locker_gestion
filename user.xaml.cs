using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace gestion
{
    public class Utilisateur
    {
        // Propriétés de l'utilisateur
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        // Ajoutez d'autres propriétés selon vos besoins
    }

    public partial class user : Window
    {
        public ObservableCollection<Utilisateur> Users { get; set; }
        private ObservableCollection<Utilisateur> AllUsers { get; set; }

        public user()
        {
            InitializeComponent();
            TxtSearchBox.TextChanged += TxtSearch_TextChanged;
            InitializeData();
            DataContext = this;
        }

        private void InitializeData()
        {
            AllUsers = new ObservableCollection<Utilisateur>
            {
                // Initialisation des utilisateurs
            };

            Users = new ObservableCollection<Utilisateur>(AllUsers);
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Crée une nouvelle instance de la fenêtre principale
            mainWindow.Show(); // Affiche la fenêtre principale
            this.Close(); // Ferme la fenêtre actuelle
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panel administrateur", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour ajouter un utilisateur
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour modifier un utilisateur
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour supprimer un utilisateur
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Logique pour la recherche
        }
    }
}