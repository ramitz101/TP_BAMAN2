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

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour EcranAccueil.xaml
    /// </summary>
    /// 
    //TESSSSTTTTTTTTTTT123
    public partial class EcranAccueil : UserControl
    {
        
        public EcranAccueil()
        {
            InitializeComponent();
        }

        private void btnInventaire_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();

            EcranInventaire Ecraninventaire = new EcranInventaire();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(Ecraninventaire);


        }


        private void btnEmploye_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();

            EcranEmploye EcranEmploye = new EcranEmploye();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EcranEmploye);
        }

        private void btnVente_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();

            EcranVente EcranVente = new EcranVente();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EcranVente);
        }

        private void btnCommande_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();

            EcranCommande EcranCommand = new EcranCommande();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EcranCommand);
        }

        private void btnFormulaireBouteille_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();

            EcranFormulaireBouteille EcranFormulaire = new EcranFormulaireBouteille();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EcranFormulaire);
        }
    }
}
