using System.Windows;

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

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            // Mettre à jour les informations de l'utilisateur
            UtilisateurAModifier.Nom = TxtNom.Text;
            UtilisateurAModifier.Prenom = TxtPrenom.Text;
            UtilisateurAModifier.Email = TxtEmail.Text;

            this.DialogResult = true; // Indique que la modification a été effectuée
            this.Close();
        }
    }
}