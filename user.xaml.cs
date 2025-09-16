using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input; // Ajouté pour InputBox alternative

namespace gestion
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Utilisateur> Users { get; set; }
        private ObservableCollection<Utilisateur> AllUsers { get; set; }
        public ListView ListViewUsers { get; set; }
        private TextBox TxtSearch; // Ajouté pour la recherche

        // Vérifiez s'il existe une autre définition du constructeur MainWindow dans ce fichier ou dans le fichier généré (user.g.cs ou user.xaml.cs partiel).
        // Supprimez ou fusionnez le constructeur en double pour qu'il n'y ait qu'une seule définition de :
        // public MainWindow()
        // Si vous avez un constructeur MainWindow ailleurs, gardez seulement celui qui correspond à votre logique principale.

        public MainWindow()
        {
            InitializeComponent();
            ListViewUsers = (ListView)this.FindName("ListViewUsers");
            TxtSearch = (TextBox)this.FindName("TxtSearch");
            InitializeData();
            DataContext = this;
        }

        private void InitializeData()
        {
            AllUsers = new ObservableCollection<Utilisateur>
            {
                new Utilisateur
                {
                    Id = 1,
                    Nom = "Jean Dupont",
                    Email = "jean.dupont@email.com",
                    CarteRFID = "RF001234",
                    EstActif = true
                },
                new Utilisateur
                {
                    Id = 2,
                    Nom = "Marie Martin",
                    Email = "marie.martin@email.com",
                    CarteRFID = "RF005678",
                    EstActif = true
                },
                new Utilisateur
                {
                    Id = 3,
                    Nom = "Pierre Durand",
                    Email = "pierre.durand@email.com",
                    CarteRFID = "RF009012",
                    EstActif = false
                }
            };

            Users = new ObservableCollection<Utilisateur>(AllUsers);
            ListViewUsers.ItemsSource = Users;
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Retour au menu principal", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ouverture du panel administrateur", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            // Remplacement de Microsoft.VisualBasic.Interaction.InputBox par une boîte de dialogue personnalisée
            var inputDialog = new InputDialog("Nom complet:");
            if (inputDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(inputDialog.ResponseText))
            {
                string nom = inputDialog.ResponseText;
                inputDialog = new InputDialog("Email:");
                if (inputDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(inputDialog.ResponseText))
                {
                    string email = inputDialog.ResponseText;
                    inputDialog = new InputDialog("Carte RFID:");
                    if (inputDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(inputDialog.ResponseText))
                    {
                        string carteRFID = inputDialog.ResponseText;
                        var nouvelUtilisateur = new Utilisateur
                        {
                            Id = AllUsers.Count + 1,
                            Nom = nom,
                            Email = email,
                            CarteRFID = carteRFID,
                            EstActif = true
                        };

                        AllUsers.Add(nouvelUtilisateur);
                        Users.Add(nouvelUtilisateur);
                        MessageBox.Show("Utilisateur ajouté avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Utilisateur utilisateur)
            {
                // Remplacement de Microsoft.VisualBasic.Interaction.InputBox par la boîte de dialogue personnalisée
                var inputDialog = new InputDialog("Nouveau nom:");
                inputDialog.inputBox.Text = utilisateur.Nom;
                if (inputDialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(inputDialog.ResponseText))
                {
                    string nouveauNom = inputDialog.ResponseText;
                    utilisateur.Nom = nouveauNom;
                    MessageBox.Show("Utilisateur modifié avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Utilisateur utilisateur)
            {
                var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {utilisateur.Nom}?",
                                           "Confirmer la suppression",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    AllUsers.Remove(utilisateur);
                    Users.Remove(utilisateur);
                    MessageBox.Show("Utilisateur supprimé avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TxtSearch.Text.ToLower();

            Users.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var user in AllUsers)
                {
                    Users.Add(user);
                }
            }
            else
            {
                var filteredUsers = AllUsers.Where(u =>
                    u.Nom.ToLower().Contains(searchText) ||
                    u.Email.ToLower().Contains(searchText) ||
                    u.CarteRFID.ToLower().Contains(searchText)
                );

                foreach (var user in filteredUsers)
                {
                    Users.Add(user);
                }
            }
        }
    }

    // Classe Utilisateur
    public class Utilisateur : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private string _email = string.Empty;
        private string _carteRFID = string.Empty;
        private bool _estActif;

        public int Id { get; set; }

        public string Nom
        {
            get => _nom;
            set
            {
                _nom = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string CarteRFID
        {
            get => _carteRFID;
            set
            {
                _carteRFID = value;
                OnPropertyChanged();
            }
        }

        public bool EstActif
        {
            get => _estActif;
            set
            {
                _estActif = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Statut));
                OnPropertyChanged(nameof(StatutColor));
                OnPropertyChanged(nameof(StatutTextColor));
                OnPropertyChanged(nameof(StatutWidth));
            }
        }

        public string Statut => EstActif ? "Actif" : "Inactif";

        public Brush StatutColor => EstActif ?
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C3E50")) :
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));

        public Brush StatutTextColor => EstActif ?
            Brushes.White :
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));

        public double StatutWidth => EstActif ? 50 : 55;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe pour la boîte de dialogue d'entrée
    public class InputDialog : Window
    {
        public TextBox inputBox; // Changer le niveau d'accès de 'private' à 'public'
        public string ResponseText => inputBox.Text;

        public InputDialog(string question)
        {
            Title = question;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Width = 300;
            Height = 120;
            ResizeMode = ResizeMode.NoResize;

            var panel = new StackPanel { Margin = new Thickness(10) };
            inputBox = new TextBox { Margin = new Thickness(0, 10, 0, 10) };
            var okButton = new Button { Content = "OK", IsDefault = true, Width = 60, Margin = new Thickness(0, 0, 10, 0) };
            okButton.Click += (s, e) => { DialogResult = true; Close(); };

            panel.Children.Add(new TextBlock { Text = question });
            panel.Children.Add(inputBox);
            panel.Children.Add(okButton);

            Content = panel;
        }
    }
}
