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
    public class Utilisateur : INotifyPropertyChanged
    {
        private string nom;
        private string prenom;
        private string email;
        private string carteRFID;
        private string statut;
        private string statutColor;
        private double statutWidth;
        private string statutTextColor;

        public string Nom
        {
            get => nom;
            set { nom = value; OnPropertyChanged(); }
        }

        public string Prenom
        {
            get => prenom;
            set { prenom = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string CarteRFID
        {
            get => carteRFID;
            set { carteRFID = value; OnPropertyChanged(); }
        }

        public string Statut
        {
            get => statut;
            set { statut = value; OnPropertyChanged(); }
        }

        public string StatutColor
        {
            get => statutColor;
            set { statutColor = value; OnPropertyChanged(); }
        }

        public double StatutWidth
        {
            get => statutWidth;
            set { statutWidth = value; OnPropertyChanged(); }
        }

        public string StatutTextColor
        {
            get => statutTextColor;
            set { statutTextColor = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
            lecteur.Port = 5; // Remplace par le port correct
            lecteur.Baud = 19200; // Taux de bauds

            int connectionStatus = lecteur.connectionRs();
            if (connectionStatus != 0)
            {
                MessageBox.Show("Erreur de connexion au lecteur RFID.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string carteID = lecteur.GetCardID();

            if (!string.IsNullOrEmpty(carteID))
            {
                AjouterUtilisateur ajouterUtilisateurWindow = new AjouterUtilisateur();
                ajouterUtilisateurWindow.TxtCarteRFID.Text = carteID;

                if (ajouterUtilisateurWindow.ShowDialog() == true)
                {
                    var nouvelUtilisateur = ajouterUtilisateurWindow.NouvelUtilisateur;
                    AllUsers.Add(nouvelUtilisateur);
                    Users.Add(nouvelUtilisateur);
                }
            }
            else
            {
                MessageBox.Show("Aucune carte RFID détectée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lecteur.fermetureRs(); // Ferme la connexion après l'utilisation
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var utilisateur = (Utilisateur)((Button)sender).Tag;
            ModifierUtilisateur modifierUtilisateurWindow = new ModifierUtilisateur(utilisateur);

            if (modifierUtilisateurWindow.ShowDialog() == true)
            {
                // Les données de l'utilisateur sont mises à jour automatiquement grâce à INotifyPropertyChanged.
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