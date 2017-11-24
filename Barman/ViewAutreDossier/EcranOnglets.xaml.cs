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
using Barman.BouteilleDossier.view;
using Barman.CommandeDossier.view;
using Barman.EmployeDossier.view;
using Barman.VenteDossier.view;

namespace Barman.ViewAutreDossier
{
    /// <summary>
    /// Interaction logic for EcranOnglets.xaml
    /// </summary>
    public partial class EcranOnglets : UserControl
    {

        private TabItem LastSelected { get; set; }
        
        private bool OngletCreer { get; set; } = false;
        public EcranOnglets(int tbiIndex)
        {
            InitializeComponent();
            switch(tbiIndex)
            {
                case 0:
                    tbcOnglet.SelectedItem = tbiInventaire;  break;
                case 1:
                    tbcOnglet.SelectedItem = tbiEmploye;  break;
                case 2:
                    tbcOnglet.SelectedItem = tbiVente; break;
                case 3:
                    tbcOnglet.SelectedItem = tbiCommande;  break;
                case 4:
                    tbcOnglet.SelectedItem = tbiFormulaireB; break;
             
            }
            OngletCreer = true;
        }


        private void tbcOnglet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Authentification auth = new Authentification();
            if (tbcOnglet.SelectedItem == tbiInventaire)
            {
                if(OngletCreer)
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranInventaire EI = new EcranInventaire();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0,EI);
                
            }
            else if (tbcOnglet.SelectedItem == tbiCommande)
            {
                if (OngletCreer)
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranCommande EC = new EcranCommande();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0,EC);
                
            }
            else if (tbcOnglet.SelectedItem == tbiEmploye)
            {                
                if (auth.ValiderRoleAdmin())
                {
                    if (OngletCreer)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranEmploye EE = new EcranEmploye();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EE);
                }
                
            }
            else if (tbcOnglet.SelectedItem == tbiVente)
            {                
                if (auth.ValiderRoleAdmin())
                {
                    if (OngletCreer)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranVente EV = new EcranVente();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EV);

                }
            }
            else if (tbcOnglet.SelectedItem == tbiFormulaireB)
            {                
                if (!(auth.ValiderRoleAdmin()))
                {
                    if (OngletCreer)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranFormulaireBouteille EFB = new EcranFormulaireBouteille();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EFB);
                }
            }            
        }          
    }
}
