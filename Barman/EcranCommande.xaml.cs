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
    /// Logique d'interaction pour EcranCommande.xaml
    /// </summary>
    public partial class EcranCommande : UserControl
    {
        public EcranCommande()
        {
            InitializeComponent();
            //tbcOnglet.SelectedItem = tbiCommande;
            //tbiCommande.IsSelected = true;

        }

        private void btnNouvelleCommande_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranNouvelleCommande EcranNouvelCommand = new EcranNouvelleCommande();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EcranNouvelCommand);
        }
    }
}
