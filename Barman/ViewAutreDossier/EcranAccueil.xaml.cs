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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Barman.EmployeDossier;

namespace Barman.ViewAutreDossier
{
    /// <summary>
    /// Logique d'interaction pour EcranAccueil.xaml
    /// </summary>
    /// 

    public partial class EcranAccueil : UserControl
    {
        static public Employe Employe {get;set;} // A la création de la fenetre accueil le role d'un utilisateur est réinitialisé
        public EcranAccueil()
        {
            InitializeComponent();
            App.Current.MainWindow.Title = Constante.APPTITLE;
            Employe = new Employe();
        }

        private void btnInventaire_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(0);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);


        }
        private void btnEmploye_Click(object sender, RoutedEventArgs e)
        {
            Authentification auth = new Authentification();
            if (auth.ValiderRoleAdmin())
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                EcranOnglets EO = new EcranOnglets(1);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
            }
            /*else if (Employe.IdRole != null)
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
                if (auth.ValiderRoleAdmin())
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                    EcranOnglets EO = new EcranOnglets(1);
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
                }
            }*/
                    
            
        }

        private void btnVente_Click(object sender, RoutedEventArgs e)
        {
            
            FenetreAuthentification FA = new FenetreAuthentification(null);
            FA.ShowDialog();            
            if (Employe.IdEmploye != null)
            {
               ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                EcranOnglets EO = new EcranOnglets(2);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);

            }
        }

        private void btnCommande_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(3);          
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnFormulaireBouteille_Click(object sender, RoutedEventArgs e)
        {
            FenetreAuthentification FA = new FenetreAuthentification(null);
            FA.ShowDialog();

            if (Employe.IdEmploye != null)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                EcranOnglets EO = new EcranOnglets(4);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
            }
        }
    }
}
