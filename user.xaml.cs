using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GestionAbsence.RFID;

namespace gestion
{
    public class Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string CarteRFID { get; set; }
        public string Statut { get; set; }
        public string StatutColor { get; set; }
        public double StatutWidth { get; set; }
        public string StatutTextColor { get; set; }
    }

    public partial class user : Window, INotifyPropertyChanged
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
                new Utilisateur { Nom = "Dupont", Prenom = "Jean", Email = "jean.dupont@example.com", CarteRFID = "123456", Statut = "Actif", StatutColor = "#4CAF50", StatutWidth = 80, StatutTextColor = "White" },
                new Utilisateur { Nom = "Martin", Prenom = "Sophie", Email = "sophie.martin@example.com", CarteRFID = "654321", Statut = "Inactif", StatutColor = "#F44336", StatutWidth = 70, StatutTextColor = "White" }
            };

            Users = new ObservableCollection<Utilisateur>(AllUsers);
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panel administrateur", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            LecteurRfid lecteur = new LecteurRfid();
            string carteID = lecteur.GetCardID();

            if (!string.IsNullOrEmpty(carteID))
            {
                AjouterUtilisateur ajouterUtilisateurWindow = new AjouterUtilisateur();
                ajouterUtilisateurWindow.TxtCarteRFID.Text = carteID;

                if (ajouterUtilisateurWindow.ShowDialog() == true)
                {
                    var nouvelUtilisateur = ajouterUtilisateurWindow.NouvelUtilisateur;
                    nouvelUtilisateur.CarteRFID = carteID;
                    AllUsers.Add(nouvelUtilisateur);
                    Users.Add(nouvelUtilisateur);
                }
            }
            else
            {
                MessageBox.Show("Aucune carte RFID détectée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var utilisateur = (Utilisateur)((Button)sender).Tag;
            ModifierUtilisateur modifierUtilisateurWindow = new ModifierUtilisateur(utilisateur);

            if (modifierUtilisateurWindow.ShowDialog() == true)
            {
                // Les données de l'utilisateur sont mises à jour, pas besoin de rafraîchir la liste.
                // ObservableCollection gère cela automatiquement.
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var utilisateur = (Utilisateur)((Button)sender).Tag;
            AllUsers.Remove(utilisateur); // Remove from the data source
            Users.Remove(utilisateur); // Remove from the view
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TxtSearchBox.Text.ToLower();
            Users.Clear();

            foreach (var user in AllUsers.Where(u => u.Nom.ToLower().Contains(searchText) || u.Email.ToLower().Contains(searchText)))
            {
                Users.Add(user);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}