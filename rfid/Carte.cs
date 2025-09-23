using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAbsence.RFID
{
    public class Carte
    {
        protected String identifiant;

        public string Identifiant
        {
            get { return identifiant; }
            set { identifiant = value; }
        }
        public Carte(string Identifiant)
        {
            this.identifiant = Identifiant;
        }

        /* public string GetCardID()
         {

             int status = lelecteur.lireIdentifiantCarte();
             string statusMsg = null;
             switch (status)
             {
                 case 0:
                     statusMsg = carteLu.Identifiant;
                     break;

                 case 1:
                     MessageBox.Show("Erreur de vitesse du Port Série", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                     break;

                 case 2:
                     MessageBox.Show("Erreur de numéro du Port Série", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                     break;

                 case 20:
                     MessageBox.Show("Carte absente", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                     break;
             }
             return statusMsg;
         }*/

    }
}
