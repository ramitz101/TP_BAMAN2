
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
    /// Logique d'interaction pour EcranVente.xaml
    /// </summary>
    public partial class EcranVente : UserControl
    {
   
        private Bouteille LaBouteilleVendu;
        private List<Bouteille> lstBouteille = new List<Bouteille>();
        private ObservableCollection<Emplacement> lstEmplacement = new ObservableCollection<Emplacement>(ChargerListEmplacement());
        public EcranVente()
        {
            InitializeComponent();
           
            // Affichage de l'employé qui a ouvert la fenêtre
            if (EcranAccueil.employe.IdRole == 1)
                lblTypeEmploye.Content = "Administrateur";
            else if(EcranAccueil.employe.IdRole == 2)
                lblTypeEmploye.Content = "Employé";
            StringBuilder s = new StringBuilder();
            s.Append(EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom);
            lblEmploye.Content = s.ToString();

            //ComboBox
           
           

            cboEmplacement.ItemsSource = lstEmplacement;
            cboEmplacement.DisplayMemberPath = "Nom";
            cboEmplacement.SelectedValuePath = "IdEmplacement";
            cboEmplacement.SelectedIndex = 0;

            //lstBouteille = ChargerListBouteille();
            cboMarque.ItemsSource = lstBouteille;
            cboMarque.DisplayMemberPath = "SaMarque.Nom";
            cboMarque.SelectedValuePath = "IdBouteille";
            cboMarque.SelectedIndex = 1;
        }

        private List<Bouteille> ChargerListBouteille()
        {
           
            List<Bouteille> listB = new List<Bouteille>(HibernateBouteilleService.RetrieveBouteilleEmplacement((int)cboEmplacement.SelectedValue));
            return listB;
        }

        private static List<Emplacement> ChargerListEmplacement()
        {
            List<Emplacement> listE = new List<Emplacement>(HibernateEmplacementService.RetrieveAll());
            return listE;
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {

            Vente v = new Vente(float.Parse(lblPrixVente.Content.ToString()), DateTime.Now, int.Parse(txtQuantite.Text), (int)LaBouteilleVendu.IdBouteille, (int)EcranAccueil.employe.IdEmploye, EcranAccueil.employe,LaBouteilleVendu);           
            HibernateVenteService.Create(v);
            
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranConsulterVente EAI = new EcranConsulterVente();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);           

        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (EcranAccueil.employe.IdRole == 1)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranGererVente EAI = new EcranGererVente();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
            }
            else
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
                if(EcranAccueil.employe.IdRole == 1)
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranGererVente EAI = new EcranGererVente();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
                }

            }
    
         }

        private void btnAugmenteQ_Click(object sender, RoutedEventArgs e)
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
            AjusterPrix();
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
            AjusterPrix();
        }

        private void cboEmplacement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboMarque.ItemsSource = ChargerListBouteille();
        }

        private void cboMarque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Bouteille> lstBouteille = new List<Bouteille>(HibernateBouteilleService.RetrieveAll());
            
            foreach(var i in lstBouteille)
            {
                if(i.SaMarque.IdMarque == (int?)cboMarque.SelectedValue)
                {
                    LaBouteilleVendu = i;
                    break;
                }
            }

            AjusterPrix();
            
        }

        private void AjusterPrix()
        {
            if (LaBouteilleVendu != null)
            {
                
                if (LaBouteilleVendu.PrixBouteille <= 40)
                {
                    lblPrixVente.Content = (6 * int.Parse(txtQuantite.Text)).ToString();
                }
                else if (LaBouteilleVendu.PrixBouteille <= 50)
                {
                    lblPrixVente.Content = (7 * int.Parse(txtQuantite.Text)).ToString();
                }
                else if (LaBouteilleVendu.PrixBouteille <= 60)
                {
                    lblPrixVente.Content = (8 * int.Parse(txtQuantite.Text)).ToString();
                }
                else if (LaBouteilleVendu.PrixBouteille <= 70)
                {
                    lblPrixVente.Content = (9 * int.Parse(txtQuantite.Text)).ToString();
                }
                else
                {
                    lblPrixVente.Content = (10 * int.Parse(txtQuantite.Text)).ToString();
                }
            }
        }
    }
}
