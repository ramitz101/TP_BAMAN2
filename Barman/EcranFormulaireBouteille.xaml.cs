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
    /// Logique d'interaction pour EcranFormulaireBouteille.xaml
    /// </summary>
    public partial class EcranFormulaireBouteille : UserControl
    {
        public EcranFormulaireBouteille()
        {
            InitializeComponent();
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
