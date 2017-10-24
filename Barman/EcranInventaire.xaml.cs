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
            lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille());
            dtgInventaire.ItemsSource = lstBouteilles;
            dtgInventaire.Items.Refresh();

            dtgInventaire.IsReadOnly = true;
        }

     
        private void btnAjout_Click(object sender, RoutedEventArgs e)
        {
            if (EcranAccueil.employe.IdRole == null)
            {
                FenetreAuthentification FN = new FenetreAuthentification();
                FN.ShowDialog();
            }
            if (EcranAccueil.employe.IdRole != 1 && EcranAccueil.employe.IdRole != null)
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
            }
            if (EcranAccueil.employe.IdRole == 1)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranAjoutInventaire EAI = new EcranAjoutInventaire();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
            }

            dtgInventaire.ItemsSource = new ObservableCollection<Bouteille>(ChargerListBouteille());
        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (UneBouteilleSelectionne())
            {

                if (EcranAccueil.employe.IdRole == null)
                {
                    FenetreAuthentification FN = new FenetreAuthentification();
                    FN.ShowDialog();
                }
                if(EcranAccueil.employe.IdRole != 1 && EcranAccueil.employe.IdRole != null)
                {
                    FenetreErreur FE = new FenetreErreur();
                    FE.ShowDialog();
                }
                if (EcranAccueil.employe.IdRole == 1)
                {
                    FenetreModifierBouteille popup = new FenetreModifierBouteille(lstBouteilles, dtgInventaire.SelectedItem as Bouteille, this);
                    popup.ShowDialog();
                }

                dtgInventaire.ItemsSource = new ObservableCollection<Bouteille>(ChargerListBouteille());
                //dtgInventaire.Items.Refresh();

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
         foreach (Bouteille b in listB)
         {
            b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
            b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
            b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];

         }

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

      private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
      {
         txtRecherche.Text = "";
         List<Bouteille> lstBouteille = HibernateBouteilleService.RetrieveAll();
         foreach (Bouteille b in lstBouteille)
         {
            b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
            b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
            b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
         }
         dtgInventaire.ItemsSource = lstBouteille;
      }

      private void txtRecherche_LostFocus(object sender, RoutedEventArgs e)
      {
         txtRecherche.Text = "Rechercher";
      }

  
    

      private void txtRecherche_KeyUp_1(object sender, KeyEventArgs e)
      {
         List<Bouteille> lstBouteille = new List<Bouteille>();
         List<Marque> lstMarque = new List<Marque>();
         if (txtRecherche.Text != "" && txtRecherche.Text != "Recherche")
         {
            lstMarque = HibernateMarqueService.Retrieve(txtRecherche.Text);
            foreach (Marque m in lstMarque)
            {
               lstBouteille.AddRange(HibernateBouteilleService.RetrieveByMarqueId((int)m.IdMarque));
            }
            

            if (lstBouteille.Count != 0)
               dtgInventaire.ItemsSource = lstBouteille;
            else
            {
               dtgInventaire.ItemsSource = null;
            }
         }
         else
         {
            lstBouteille = HibernateBouteilleService.RetrieveAll();
            foreach (Bouteille b in lstBouteille)
            {
               b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
               b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
               b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
            }

            dtgInventaire.ItemsSource = lstBouteille;
         }
      }
      private void btnImprimer_Click(object sender, RoutedEventArgs e)
      {
         MessageBox.Show("Bravo, vous avez imprimer avec succès!");
      }
   }
}
