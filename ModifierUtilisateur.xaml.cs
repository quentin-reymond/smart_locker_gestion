using System.Windows;
using GestionAbsence.RFID;

namespace gestion
{
    public partial class ModifierUtilisateur : Window
    {
        public Utilisateur UtilisateurAModifier { get; set; }

        public ModifierUtilisateur(Utilisateur utilisateur)
        {
            InitializeComponent();
            UtilisateurAModifier = utilisateur;

            // Remplir les champs avec les informations de l'utilisateur
            TxtNom.Text = utilisateur.Nom;
            TxtPrenom.Text = utilisateur.Prenom;
            TxtEmail.Text = utilisateur.Email;
            TxtCarteRFID.Text = utilisateur.CarteRFID;
        }

        private void BtnScanRFID_Click(object sender, RoutedEventArgs e)
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
                TxtCarteRFID.Text = carteID; // Met à jour le champ avec l'ID scanné
            }
            else
            {
                MessageBox.Show("Aucune carte RFID détectée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lecteur.fermetureRs(); // Ferme la connexion après l'utilisation
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            // Mettre à jour les informations de l'utilisateur
            UtilisateurAModifier.Nom = TxtNom.Text;
            UtilisateurAModifier.Prenom = TxtPrenom.Text;
            UtilisateurAModifier.Email = TxtEmail.Text;
            UtilisateurAModifier.CarteRFID = TxtCarteRFID.Text; // Met à jour la carte RFID

            this.DialogResult = true; // Indique que la modification a été effectuée
            this.Close();
        }
    }
}