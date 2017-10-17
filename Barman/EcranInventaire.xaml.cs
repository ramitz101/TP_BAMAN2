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
            if (UneBouteilleSelectionne())
            {
                if (EcranAccueil.employe.IdRole == 1)
                {
                    FenetreModifierBouteille popup = new FenetreModifierBouteille(lstBouteilles, dtgInventaire.SelectedItem as Bouteille, this);
                    popup.ShowDialog();
                }
                else if(EcranAccueil.employe.IdRole == null)
                {
                    FenetreAuthentification FN = new FenetreAuthentification();
                    FN.ShowDialog();
                    if(EcranAccueil.employe.IdRole == 1)
                    {
                        FenetreModifierBouteille popup = new FenetreModifierBouteille(lstBouteilles, dtgInventaire.SelectedItem as Bouteille, this);
                        popup.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Désoler,cette fonctionalité nécéssite un administrateur", "Accès refuser", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                }
                else
                    MessageBox.Show("Désoler,cette fonctionalité nécéssite un administrateur", "Accès refuser", MessageBoxButton.OK, MessageBoxImage.Warning);

                dtgInventaire.Items.Refresh();

            }

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

        

        private bool UneBouteilleSelectionne()
        {
            if (dtgInventaire.SelectedItems.Count == 1)
                return true;
            // si le user a sélectionné plus d'une inscription 
            else if (dtgInventaire.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vous avez trops de bouteilles selectionnées");
                return false;
            }
            else
            {
                MessageBox.Show("Vous devez selectionner une bouteille");
                return false;
            }
        }

    }
}
