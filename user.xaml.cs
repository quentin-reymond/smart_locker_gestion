using System;
using System.Collections.ObjectModel; // Pour utiliser ObservableCollection
using System.ComponentModel; // Pour INotifyPropertyChanged
using System.Linq; // Pour LINQ
using System.Runtime.CompilerServices; // Pour CallerMemberName
using System.Windows; // Pour Window et MessageBox
using System.Windows.Controls; // Pour les contrôles WPF
using GestionAbsence.RFID; // Espace de noms pour le module RFID

namespace gestion
{
    // Classe représentant un utilisateur
    public class Utilisateur : INotifyPropertyChanged
    {
        // Propriétés privées
        private string nom;
        private string prenom;
        private string email;
        private string carteRFID;
        private string statut;
        private string statutColor;
        private double statutWidth;
        private string statutTextColor;

        // Propriétés publiques avec notification de changement
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

        // Événement pour notifier les changements de propriété
        public event PropertyChangedEventHandler PropertyChanged;

        // Méthode pour lever l'événement de changement de propriété
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe représentant la fenêtre de gestion des utilisateurs
    public partial class user : Window, INotifyPropertyChanged
    {
        // Collection observable d'utilisateurs
        public ObservableCollection<Utilisateur> Users { get; set; }
        private ObservableCollection<Utilisateur> AllUsers { get; set; }

        // Constructeur de la fenêtre
        public user()
        {
            InitializeComponent(); // Initialise les composants de la fenêtre
            TxtSearchBox.TextChanged += TxtSearch_TextChanged; // Événement pour la recherche
            InitializeData(); // Initialise les données des utilisateurs
            DataContext = this; // Définit le contexte de données pour le binding
        }

        // Méthode pour initialiser les données des utilisateurs
        private void InitializeData()
        {
            AllUsers = new ObservableCollection<Utilisateur>
            {
                // Ajout d'utilisateurs par défaut
                new Utilisateur { Nom = "Dupont", Prenom = "Jean", Email = "jean.dupont@example.com", CarteRFID = "123456", Statut = "Actif", StatutColor = "#4CAF50", StatutWidth = 80, StatutTextColor = "White" },
                new Utilisateur { Nom = "Martin", Prenom = "Sophie", Email = "sophie.martin@example.com", CarteRFID = "654321", Statut = "Inactif", StatutColor = "#F44336", StatutWidth = 70, StatutTextColor = "White" }
            };

            Users = new ObservableCollection<Utilisateur>(AllUsers); // Copie des utilisateurs dans la collection visible
        }

        // Événement pour le bouton de retour
        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Crée une nouvelle instance de MainWindow
            mainWindow.Show(); // Affiche la fenêtre principale
            this.Close(); // Ferme la fenêtre actuelle
        }

        // Événement pour le bouton d'administration
        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panel administrateur", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Événement pour le bouton d'ajout d'utilisateur
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            LecteurRfid lecteur = new LecteurRfid(); // Crée une instance du lecteur RFID
            lecteur.Port = 5; // Définit le port de connexion
            lecteur.Baud = 19200; // Définit le taux de bauds

            int connectionStatus = lecteur.connectionRs(); // Essaie de se connecter au lecteur
            if (connectionStatus != 0)
            {
                // Affiche un message d'erreur si la connexion échoue
                MessageBox.Show("Erreur de connexion au lecteur RFID.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Récupère l'ID de la carte RFID
            string carteID = lecteur.GetCardID();

            if (!string.IsNullOrEmpty(carteID)) // Vérifie si une carte RFID a été détectée
            {
                AjouterUtilisateur ajouterUtilisateurWindow = new AjouterUtilisateur(); // Ouvre la fenêtre d'ajout d'utilisateur
                ajouterUtilisateurWindow.TxtCarteRFID.Text = carteID; // Remplit le champ avec l'ID de la carte

                // Affiche la fenêtre et vérifie si l'utilisateur a validé
                if (ajouterUtilisateurWindow.ShowDialog() == true)
                {
                    var nouvelUtilisateur = ajouterUtilisateurWindow.NouvelUtilisateur; // Récupère le nouvel utilisateur
                    AllUsers.Add(nouvelUtilisateur); // Ajoute à la liste totale
                    Users.Add(nouvelUtilisateur); // Ajoute à la liste visible
                }
            }
            else
            {
                // Affiche un message d'erreur si aucune carte n'est détectée
                MessageBox.Show("Aucune carte RFID détectée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lecteur.fermetureRs(); // Ferme la connexion après utilisation
        }

        // Événement pour le bouton de modification d'utilisateur
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var utilisateur = (Utilisateur)((Button)sender).Tag; // Récupère l'utilisateur associé au bouton cliqué
            ModifierUtilisateur modifierUtilisateurWindow = new ModifierUtilisateur(utilisateur); // Ouvre la fenêtre de modification

            // Affiche la fenêtre et vérifie si l'utilisateur a validé
            if (modifierUtilisateurWindow.ShowDialog() == true)
            {
                // Les données de l'utilisateur sont mises à jour automatiquement grâce à INotifyPropertyChanged.
            }
        }

        // Événement pour le bouton de suppression d'utilisateur
        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            var utilisateur = (Utilisateur)((Button)sender).Tag; // Récupère l'utilisateur associé au bouton
            AllUsers.Remove(utilisateur); // Supprime de la source de données
            Users.Remove(utilisateur); // Supprime de la vue
        }

        // Événement pour le changement de texte dans la zone de recherche
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TxtSearchBox.Text.ToLower(); // Récupère le texte de recherche en minuscule
            Users.Clear(); // Vide la liste visible

            // Ajoute les utilisateurs qui correspondent à la recherche
            foreach (var user in AllUsers.Where(u => u.Nom.ToLower().Contains(searchText) || u.Email.ToLower().Contains(searchText)))
            {
                Users.Add(user); // Ajoute à la liste visible
            }
        }

        // Événement pour notifier les changements de propriété
        public event PropertyChangedEventHandler PropertyChanged;

        // Méthode pour lever l'événement de changement de propriété
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}