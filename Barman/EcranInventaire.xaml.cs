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
    /// Logique d'interaction pour inventaire.xaml
    /// </summary>
    public partial class EcranInventaire : UserControl
    {
        private static ObservableCollection<Bouteille> lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille());
        public EcranInventaire()
        {
            InitializeComponent();
            dtgInventaire.ItemsSource = lstBouteilles;
        }

     
        private void btnAjout_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranAjoutInventaire EAI = new EcranAjoutInventaire();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0,EAI);
        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            FenetreModifierBouteille popup = new FenetreModifierBouteille();
            popup.ShowDialog();

        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        
        // fonction charger tous les bouteille de l'inventaire (chargerListeBouteille) elle retourne la liste de la BD
        private static List<Bouteille> ChargerListBouteille()
        {
            List<Bouteille> listB = new List<Bouteille>(HibernateBouteilleService.RetrieveAll());
            return listB;
        }

    }
}
