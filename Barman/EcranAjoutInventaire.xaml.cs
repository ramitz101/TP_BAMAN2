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
    /// Logique d'interaction pour EcranAjoutInventaire.xaml
    /// </summary>
    public partial class EcranAjoutInventaire : UserControl
    {

        private ObservableCollection<Bouteille> lstBouteilles = new ObservableCollection<Bouteille>();
        private ObservableCollection<Marque> lstMarques = new ObservableCollection<Marque>();

        public EcranAjoutInventaire()
        {
            InitializeComponent();

            
           
        }

      

        private void btnReduireQ_Click(object sender, RoutedEventArgs e)
        {
            int quantite;
            try
            {
                quantite = Int32.Parse(txtQuantite.Text);
                if (quantite > 1)
                    quantite--;
                txtQuantite.Text = quantite.ToString();
            }
            catch (Exception ex)
            {
                quantite = 1;
                txtQuantite.Text = quantite.ToString();
            }
        }

        private void btnAugmenterQ_Click(object sender, RoutedEventArgs e)
        {
            int quantite;
            try
            {
                quantite = Int32.Parse(txtQuantite.Text);
                quantite++;
                txtQuantite.Text = quantite.ToString();
            }
            catch (Exception ex)
            {
                quantite = 1;
                txtQuantite.Text = quantite.ToString();
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(0);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            //lstBouteilles.Add(new Bouteille((Marque)cboMarque.SelectedItem, int.Parse(txtVolume.Text), txtCodeSAQ.Text));

            
            //((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            //EcranOnglets EO = new EcranOnglets(0);
            //((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }
    }
}
