using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour EcranCommande.xaml
    /// </summary>
    public partial class EcranCommande : UserControl
    {
        private static ObservableCollection<Commande> lstCommandes = new ObservableCollection<Commande>(ChargerListCommande());

        private static List<Commande> ChargerListCommande()
        {
            List<Commande> listC = new List<Commande>(HibernateCommandeService.RetrieveAll());
            return listC;
        }

        public EcranCommande()
        {
            InitializeComponent();
            
            dtgCommande.ItemsSource = lstCommandes;
        }

        private void btnNouvelleCommande_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranNouvelleCommande EcranNouvelCommand = new EcranNouvelleCommande();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EcranNouvelCommand);
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }
    }
}
