using System;
using System.Windows;
using System.Windows.Controls;
using GestionAbsence.RFID; // Assurez-vous d'importer le bon espace de noms

namespace gestion
{
    public partial class MainWindow : Window
    {
        private LecteurRfid lecteurRfid; // Instance du lecteur RFID

        public MainWindow()
        {
            InitializeComponent();
            lecteurRfid = new LecteurRfid(); // Initialiser le lecteur RFID
            lecteurRfid.Port = 5; // Remplacez par le port correct
            lecteurRfid.connectionRs(); // Connecte le lecteur RFID
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

        private void BtnScanRfid_Click(object sender, RoutedEventArgs e)
        {
            // Lire l'ID de la carte RFID lorsque le bouton est cliqué
            string cardId = lecteurRfid.GetCardID();
            if (!string.IsNullOrEmpty(cardId))
            {
                // Publier l'ID de la carte via MQTT
                lecteurRfid.PublishCardId(cardId);
                MessageBox.Show($"Carte RFID lue : {cardId}");
            }
            else
            {
                MessageBox.Show("Aucune carte détectée.");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Fermer la connexion au lecteur RFID lorsque la fenêtre est fermée
            lecteurRfid.fermetureRs();
            base.OnClosed(e);
        }
    }
}