
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Barman.BouteilleDossier;
using Barman.BouteilleDossier.Hibernate;
using Barman.EmplacementDossier;
using Barman.EmplacementDossier.Hibernate;
using Barman.MarqueDossier.Hibernate;
using Barman.VenteDossier.Hibernate;
using Barman.ViewAutreDossier;

namespace Barman.VenteDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranVente.xaml
    /// </summary>
    public partial class EcranVente : UserControl
    {
        private int prix {get;set; }
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
            

            btnAjouter.IsEnabled = false;
            cboMarque.IsEnabled = false;

            //ComboBox 
            lstEmplacement.Remove(lstEmplacement.Where(x => x.Nom == "Aucun").ToList()[0]);
            lstEmplacement.Remove(lstEmplacement.Where(x => x.Nom == "Réserve").ToList()[0]);


            cboEmplacement.ItemsSource = lstEmplacement;
            cboEmplacement.DisplayMemberPath = "Nom";
            cboEmplacement.SelectedValuePath = "IdEmplacement";


          
            cboMarque.DisplayMemberPath = "SaMarque.Nom";
            cboMarque.SelectedValuePath = "IdBouteille";
            
        }

        private List<Bouteille> ChargerListBouteille()
        {
            
            List<Bouteille> listB = new List<Bouteille>(HibernateBouteilleService.RetrieveBouteilleEmplacement((int)cboEmplacement.SelectedValue));
            foreach(var i in listB)
            {
                i.SaMarque = HibernateMarqueService.Retrieve((int)i.IdMarque)[0];               
            }
            return listB;
        }

        private static List<Emplacement> ChargerListEmplacement()
        {
            List<Emplacement> listE = new List<Emplacement>(HibernateEmplacementService.RetrieveAll());
            return listE;
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LaBouteilleVendu.VolumeRestant < int.Parse(txtQuantite.Text))
                {
                    lblComfirmationAjout.Foreground = Brushes.Red;
                    lblComfirmationAjout.Content = "Erreur, quantité restante insuffisante";

                }
                else if( LaBouteilleVendu.IdEmplacement == 9)
                {
                    lblComfirmationAjout.Foreground = Brushes.Red;
                    lblComfirmationAjout.Content = "Erreur, cette bouteille n'existe plus";
                }
                else
                {
                    
                    Vente v = new Vente((prix/ int.Parse(txtQuantite.Text)), DateTime.Now, int.Parse(txtQuantite.Text), (int)LaBouteilleVendu.IdBouteille, (int)EcranAccueil.employe.IdEmploye);
                    HibernateVenteService.Create(v);
                    lblComfirmationAjout.Foreground = Brushes.Green;
                    lblComfirmationAjout.Content = "Vente ajoutée";

                    if (LaBouteilleVendu.Etat == "Pleine")
                        LaBouteilleVendu.Etat = "Entamée";

                    LaBouteilleVendu.VolumeRestant -= v.Volume;

                    if (LaBouteilleVendu.VolumeRestant == 0)
                    {
                        LaBouteilleVendu.Etat = "Vide";
                        LaBouteilleVendu.IdEmplacement = 9;

                    }

                    cboEmplacement.SelectedValue = null;
                    cboMarque.SelectedValue = null;
                    lblPrixVente.Content = "";
                    txtQuantite.Text = "1";
                    btnAjouter.IsEnabled = false;
                    HibernateBouteilleService.Update(LaBouteilleVendu);

                }            

            }
            catch(Exception z)
            {
                //MessageBox.Show(z.ToString());
                lblComfirmationAjout.Foreground = Brushes.Red;
                lblComfirmationAjout.Content = "Une erreur est survénu";
            }
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
            if (EcranAccueil.employe.SonRole.Code == "Admin")
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranGererVente EAI = new EcranGererVente();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
            }
            else
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
                if(EcranAccueil.employe.SonRole.Code == "Admin")
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
            LaBouteilleVendu = null;
            cboMarque.IsEnabled = true;
            btnAjouter.IsEnabled = false;
        }

        private void cboMarque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Bouteille> lstBouteille = new List<Bouteille>(HibernateBouteilleService.RetrieveAll(false));
            
            foreach(var i in lstBouteille)
            {
                if(i.IdBouteille == (int?)cboMarque.SelectedValue)
                {
                    LaBouteilleVendu = i;
                    break;
                }
            }
            AjusterPrix();

            btnAjouter.IsEnabled = true;
            
        }

        private void AjusterPrix()
        {
            
            if (LaBouteilleVendu != null)
            {
                // FAIRE UN FOR 
                if (LaBouteilleVendu.PrixBouteille <= 40)
                {
                    prix = 6 * int.Parse(txtQuantite.Text);
                    lblPrixVente.Content = (6 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");                
                }
                else if (LaBouteilleVendu.PrixBouteille <= 50)
                {
                    prix = 7 * int.Parse(txtQuantite.Text);
                    lblPrixVente.Content = (7 * int.Parse(txtQuantite.Text)).ToString("0.00"+ " $");
                }
                else if (LaBouteilleVendu.PrixBouteille <= 60)
                {
                    prix = 8 * int.Parse(txtQuantite.Text);
                    lblPrixVente.Content = (8 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                }
                else if (LaBouteilleVendu.PrixBouteille <= 70)
                {
                    prix = 9 * int.Parse(txtQuantite.Text);
                    lblPrixVente.Content = (9 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                }
                else
                {
                    prix = 10 * int.Parse(txtQuantite.Text);
                    lblPrixVente.Content = (10 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                }

               
            }
        }
    }
}
