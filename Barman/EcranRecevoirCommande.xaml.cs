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
        private ObservableCollection<Bouteille> listBouteilleCommand;
        
        public EcranRecevoirCommande(Commande c)
        {
            InitializeComponent();

            listBouteilleCommand = new ObservableCollection<Bouteille>(ChargerBouteilleCommande((int)c.IdCommande));
            dtgCommande.ItemsSource = listBouteilleCommand;
            

            
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

        static List<Bouteille> ChargerBouteilleCommande(int IdCommande)
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
            if(dtgCommande.SelectedCells.Count >= 1)
            {
                for(int i =0; i < dtgCommande.SelectedCells.Count;i++)
                {
                    listBouteilleCommand.ElementAt(dtgCommande.SelectedIndex + i);
                }


            }
            else
            {
                MessageBox.Show("Vous devez sélectionner une bouteille pour supprimer", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
