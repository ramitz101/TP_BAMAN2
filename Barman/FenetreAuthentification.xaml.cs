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

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour FenetreAuthentification.xaml
    /// </summary>
    public partial class FenetreAuthentification : Window
    {
        
        public FenetreAuthentification()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            txtCode.Focus();
           
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            Employe em = new Employe();
            em = Authentification();

            if (em.IdEmploye != null)
            {
                EcranAccueil.employe = em;
                this.Close();

            }
        }

        private Employe Authentification()
        {
            try
            {
                List<Employe> listEmploye = new List<Employe>(HibernateEmployeService.RetrieveAll());
        
                foreach(var i in listEmploye)
                {
                    if(i.CodeEmploye == txtCode.Text)
                    {
                        listEmploye = new List<Employe>(HibernateEmployeService.Retrieve((int)i.IdEmploye));
                        return listEmploye.ElementAt(0);
                    }
                }

            }
            catch(Exception e)
            {
                lblErreur.Content = "Erreur, le code n'existe pas";
                return new Employe();
            }
            lblErreur.Content = "Erreur, le code n'existe pas";
            return new Employe();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnConfirmer_Click(this, new RoutedEventArgs());
            }
        }
    }
}
