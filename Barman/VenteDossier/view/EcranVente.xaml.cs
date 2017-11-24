
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
using Barman.TypeDossier.Hibernate;
using Barman.TypeDossier;
using Barman.MarqueDossier;

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
            
            // Disable le bouton ajouter et les combo box
            btnAjouter.IsEnabled = false;
            cboMarque.IsEnabled = false;
            cboType.IsEnabled = false;

            //ComboBox emplacement, il faut enlevé les emplacement non nécéssaire lors d'une vente 
            lstEmplacement.Remove(lstEmplacement.Where(x => x.Nom == "Aucun").ToList()[0]);
            lstEmplacement.Remove(lstEmplacement.Where(x => x.Nom == "Réserve").ToList()[0]);


            cboEmplacement.ItemsSource = lstEmplacement;
            cboEmplacement.DisplayMemberPath = "Nom";
            cboEmplacement.SelectedValuePath = "IdEmplacement";

            cboType.DisplayMemberPath = "Nom";
            cboType.SelectedValuePath = "IdType";


            cboMarque.DisplayMemberPath = "SaMarque.Nom";
            cboMarque.SelectedValuePath = "IdBouteille";

            if (EcranAccueil.employe.SonRole.Code == "Utils")
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom + " - " + "Utilisateur" + " - Ventes";
            else
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom + " - " + "Administrateur" + " - Ventes";


        }

        private List<Bouteille> ChargerListBouteille()
        {
            
            List<Bouteille> listB = new List<Bouteille>(HibernateBouteilleService.RetrieveBouteilleEmplacement((int)cboEmplacement.SelectedValue));
            foreach(var i in listB)
            {
                i.SaMarque = HibernateMarqueService.Retrieve((int)i.IdMarque)[0];
                i.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)i.SaMarque.IdTypeAlcool)[0];
            }
            return listB;
        }

        private List<TypeAlcool> ChargerListType()
        {

            List<TypeAlcool> listT = new List<TypeAlcool>();
            foreach (var i in ChargerListBouteille())
            {
                if (!listT.Contains(i.SaMarque.SonTypeAlcool))
                    listT.Add(i.SaMarque.SonTypeAlcool);
            }
            return listT;
        }
        private List<Bouteille> ChargerListMarque()
        {
           
            List<Bouteille> listB = new List<Bouteille>(ChargerListBouteille());
            List<Bouteille> listBouteille = new List<Bouteille>();
            foreach(var i in listB)
            {
                if (i.SaMarque.SonTypeAlcool == (TypeAlcool)cboType.SelectedItem)
                    listBouteille.Add(i);
            }
            return listBouteille;
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
                // Vérification si la bouteille contient le volume à enlever
                if (LaBouteilleVendu.VolumeRestant < int.Parse(txtQuantite.Text))
                {
                    lblComfirmationAjout.Foreground = Brushes.Red;
                    lblComfirmationAjout.Content = "Erreur, quantité restante insuffisante";
                }               
                else
                {
                    
                    Vente v = new Vente((prix/ int.Parse(txtQuantite.Text)), DateTime.Now, int.Parse(txtQuantite.Text), (int)LaBouteilleVendu.IdBouteille, (int)EcranAccueil.employe.IdEmploye);
                    HibernateVenteService.Create(v);
                    lblComfirmationAjout.Foreground = Brushes.Green;
                    lblComfirmationAjout.Content = "Vente ajoutée";

                    // Enlève la quantité d'alcool vendu dans la bouteille
                    LaBouteilleVendu.VolumeRestant -= v.Volume;

                    // Changement d'état de la bouteille si pleine ou si rendu vide
                    if (LaBouteilleVendu.Etat == "Pleine")
                        LaBouteilleVendu.Etat = "Entamée";
                    else if(LaBouteilleVendu.VolumeRestant == 0)
                    {
                        LaBouteilleVendu.Etat = "Vide";
                        LaBouteilleVendu.IdEmplacement = HibernateEmplacementService.retrieveEmplacementByNom("Aucun")[0].IdEmplacement;
                    }

                    HibernateBouteilleService.Update(LaBouteilleVendu);
                    
                    
                    //Remet tous les champs à défaut
                    cboEmplacement.SelectedValue = null;
                    cboMarque.SelectedValue = null;
                    lblPrixVente.Content = "";
                    txtQuantite.Text = "1";
                    btnAjouter.IsEnabled = false;
                    cboMarque.IsEnabled = false;
                    cboType.IsEnabled = false;

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

      

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranGererVente EAI = new EcranGererVente();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
            
    
         }

        private void btnAugmenteQ_Click(object sender, RoutedEventArgs e)
        {
            int quantite;
            try
            {
                lblTitreQuantite.Content ="Quantité (MAX:"+ LaBouteilleVendu.VolumeRestant.ToString() + "oz)";
                if (LaBouteilleVendu.VolumeRestant >= int.Parse(txtQuantite.Text) + 1)
                {
                    quantite = Int32.Parse(txtQuantite.Text);
                    quantite++;
                    txtQuantite.Text = quantite.ToString();
                }
              
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
            cboType.ItemsSource = ChargerListType();
            LaBouteilleVendu = null;
            cboType.IsEnabled = true;
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
                
                if (LaBouteilleVendu.PrixBouteille <= 40)
                {
                    prix = 6 * int.Parse(txtQuantite.Text);
                    lblPrixTotal.Content = (6.00 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $"); 
                    lblPrixVente.Content = (6.00.ToString("0.00" + " $ /oz"));
                }
                else if (LaBouteilleVendu.PrixBouteille <= 50)
                {
                    prix = 7 * int.Parse(txtQuantite.Text);
                    lblPrixTotal.Content = (7.00 * int.Parse(txtQuantite.Text)).ToString("0.00"+ " $");
                    lblPrixVente.Content = (7.00.ToString("0.00" + " $ /oz"));
                }
                else if (LaBouteilleVendu.PrixBouteille <= 60)
                {
                    prix = 8 * int.Parse(txtQuantite.Text);
                    lblPrixTotal.Content = (8.00 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                    lblPrixVente.Content = (8.00.ToString("0.00" + " $ /oz"));
                }
                else if (LaBouteilleVendu.PrixBouteille <= 70)
                {
                    prix = 9 * int.Parse(txtQuantite.Text);
                    lblPrixTotal.Content = (9.00 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                    lblPrixVente.Content = (9.00.ToString("0.00" + " $ /oz"));
                }
                else
                {
                    prix = 10 * int.Parse(txtQuantite.Text);
                    lblPrixTotal.Content = (10.00 * int.Parse(txtQuantite.Text)).ToString("0.00" + " $");
                    lblPrixVente.Content = (10.00.ToString("0.00" + " $ /oz"));
                }

               
            }
        }

        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboMarque.ItemsSource = ChargerListMarque();
            cboMarque.IsEnabled = true;
        }
    }
}
