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
    /// Logique d'interaction pour EcranRecevoirCommande.xaml
    /// </summary>
    public partial class EcranRecevoirCommande : UserControl
    {
        private ObservableCollection<Bouteille> listBouteilleCommand = new ObservableCollection<Bouteille>(ChargerBouteilleCommande());
        static private int IdCommande { get; set; }
        public EcranRecevoirCommande(Commande c)
        {
            InitializeComponent();
            dtgCommande.ItemsSource = listBouteilleCommand;
            IdCommande = (int)c.IdCommande;

            
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(3);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(3);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        static List<Bouteille> ChargerBouteilleCommande()
        {
            List<Bouteille> listB = new List<Bouteille>();

            listB = HibernateBouteilleService.RetrieveByIdCommande(IdCommande);
            foreach(var i in listB)
            {
                i.SaMarque = HibernateMarqueService.Retrieve((int)i.IdMarque)[0];               
            }
            return listB;

            
               
        }


        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
