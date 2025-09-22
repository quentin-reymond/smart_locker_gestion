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
            NouvelUtilisateur = new Utilisateur
            {
                Nom = TxtNom.Text,
                Prenom = TxtPrenom.Text,
                Email = TxtEmail.Text,
                CarteRFID = TxtCarteRFID.Text
            };

            this.DialogResult = true; // Indique que l'utilisateur a été ajouté avec succès
            this.Close();
        }
    }
}