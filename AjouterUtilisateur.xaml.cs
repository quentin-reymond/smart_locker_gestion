using System.Windows;

namespace gestion
{
    public partial class AjouterUtilisateur : Window
    {
        public Utilisateur NouvelUtilisateur { get; private set; }

        public AjouterUtilisateur()
        {
            InitializeComponent();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie que tous les champs sont remplis
            if (string.IsNullOrWhiteSpace(TxtNom.Text) ||
                string.IsNullOrWhiteSpace(TxtPrenom.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtCarteRFID.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NouvelUtilisateur = new Utilisateur
            {
                Nom = TxtNom.Text,
                Prenom = TxtPrenom.Text,
                Email = TxtEmail.Text,
                CarteRFID = TxtCarteRFID.Text,
                Statut = "Actif", // Par défaut, le statut peut être "Actif"
                StatutColor = "#4CAF50", // Couleur par défaut
                StatutWidth = 80, // Largeur par défaut
                StatutTextColor = "White" // Couleur du texte par défaut
            };

            this.DialogResult = true; // Indique que l'utilisateur a été ajouté avec succès
            this.Close();
        }
    }
}