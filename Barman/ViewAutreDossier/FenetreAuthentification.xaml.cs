using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Barman.EmployeDossier;
using Barman.EmployeDossier.Hibernate;
using Barman.RoleDossier.Hibernate;
using System.Data.SqlClient;

namespace Barman.ViewAutreDossier
{
    /// <summary>
    /// Logique d'interaction pour FenetreAuthentification.xaml
    /// </summary>
    public partial class FenetreAuthentification : Window
    {
        private string RoleDemander { get; set; }
        private Authentification VerifierConfirmer { get; set; }
        public FenetreAuthentification(Authentification classAuthentification, string pRoleDemander)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            pwdBox.Focus();

            RoleDemander = pRoleDemander;
            VerifierConfirmer = classAuthentification;
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            //EcranOnglets ecranOnglets = new EcranOnglets(0);
            //ecranOnglets.tbcOnglet.SelectedItem = ecranOnglets.tbiInventaire;
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            //Authentification auth = new Authentification();
            if (ValiderAuthentification())
            {


                if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                    App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Administrateur";
                else
                    App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Utilisateur";

                VerifierConfirmer.Confirmer = true;

                this.Close();

            }
        }

        private bool ValiderAuthentification()
        {
            List<Employe> listEmploye = new List<Employe>();

            if (ValiderConnection())
            {
                listEmploye = HibernateEmployeService.RetrieveAll(null);

                if (ValiderCodeEmploye(listEmploye, pwdBox.Password.ToString()))
                {
                    if (ValideRoleDemmander())                    
                        return true;                    
                    else
                    {
                        txtErreur.Text = "Erreur, cette foncionnalitée demande un code administrateur";
                        pwdBox.SelectAll();
                        return false;
                    }
                }
                else
                {
                    txtErreur.Text = "Erreur, le code n'existe pas";
                    pwdBox.SelectAll();
                    return false;
                }
            }
            else
            {
                txtErreur.Text = "Erreur de connexion à la base de données";
                return false;
            }
        }

        private bool ValideRoleDemmander()
        {
            if (EcranAccueil.Employe.SonRole.Code == RoleDemander)            
                return true;            
            else
                return false;
        }

        private bool ValiderCodeEmploye(List<Employe> listEmploye, string codeEntre)
        {

            foreach (var e in listEmploye)
            {
                if (e.CodeEmploye == codeEntre)
                {                   
                    EcranAccueil.Employe = e;
                    return true;
                }
            }
            return false;
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnConfirmer_Click(this, new RoutedEventArgs());
        }
        private bool ValiderConnection()
        {
            try
            {
                NHibernateConnexion.OpenSession();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
