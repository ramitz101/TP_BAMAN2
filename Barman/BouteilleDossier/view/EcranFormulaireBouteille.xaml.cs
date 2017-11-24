using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Barman.BouteilleDossier.Hibernate;
using Barman.EmplacementDossier;
using Barman.EmplacementDossier.Hibernate;
using Barman.EmployeDossier;
using Barman.EmployeDossier.Hibernate;
using Barman.MarqueDossier;
using Barman.MarqueDossier.Hibernate;
using Barman.TypeDossier;
using Barman.TypeDossier.Hibernate;
using Barman.ViewAutreDossier;


namespace Barman.BouteilleDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranFormulaireBouteille.xaml
    /// </summary>
    public partial class EcranFormulaireBouteille : UserControl
    {
        private List<Bouteille> lstBouteilles = new List<Bouteille>();
        private List<TypeAlcool> lstType = new List<TypeAlcool>();
        private List<Emplacement> lstEmplacements = new List<Emplacement>(HibernateEmplacementService.RetrieveAllForFormulaire());
        private List<Marque> lstMarques = new List<Marque>();
        private Employe EmployeConnecter = new Employe();
        private List<int?> lstIdMarquesEnReserve;

        public EcranFormulaireBouteille()
        {
            InitializeComponent();
            lstIdMarquesEnReserve = new List<int?>(HibernateBouteilleService.RetrieveIdMarqueEnReserve());
            //lstIdTypeEnReserve = new List<int?>();

         lstType = PeuplerListeTypeAlcool();
           
            cboType.ItemsSource = lstType;
            cboType.DisplayMemberPath = "Nom";
            cboEmplacement.ItemsSource = lstEmplacements;
            cboEmplacement.DisplayMemberPath = "Nom";
            //lstEmployes = HibernateEmployeService.Retrieve(EcranAccueil.employe.IdEmploye);
            lblEmployeConnecte.Content = EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom;
        

         

         cboEmplacement.ItemsSource = lstEmplacements;
         cboEmplacement.DisplayMemberPath = "Nom";

        //Je vais chercher l'employé connecté pour savoir qui monte la bouteille
         EmployeConnecter = HibernateEmployeService.Retrieve(EcranAccueil.employe.IdEmploye)[0];
        //Je met le nom de l'employé connecter dans le label
         lblEmployeConnecte.Content = EmployeConnecter.Prenom + " " + EmployeConnecter.Nom;

      }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            if (FormulaireRempli())
            {

                Bouteille bouteilleAChanger = HibernateBouteilleService.RetrieveByUnique((int)HibernateMarqueService.Retrieve(cboMarqueBouteille.Text)[0].IdMarque, (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement, cboÉtiquette.Text)[0];
                if (chbVendu.IsChecked == false)
                    bouteilleAChanger.IdEmplacement = (int)HibernateEmplacementService.retrieveEmplacementByNom(cboEmplacement.Text)[0].IdEmplacement;
                else
                {
                    bouteilleAChanger.IdEmplacement = (int)HibernateEmplacementService.retrieveEmplacementByNom("Aucun")[0].IdEmplacement;
                    bouteilleAChanger.Etat = "Vendue";
                }

                HibernateBouteilleService.Update(bouteilleAChanger);
                MessageBox.Show("Modification éffectuée avec succès!", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Information);

                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, new EcranFormulaireBouteille());
            }
            else
                MessageBox.Show("Refusé");
        }

        private void cboMarqueBouteille_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboÉtiquette.IsEnabled = true;
            cboÉtiquette.ItemsSource = lstBouteilles;
            cboÉtiquette.DisplayMemberPath = "Numero";

            if (cboMarqueBouteille.SelectedItem != null)
                lstBouteilles = HibernateBouteilleService.RetrieveByMarque((Marque)cboMarqueBouteille.SelectedItem);

            if (cboÉtiquette.Items.Count == 0 && cboMarqueBouteille.SelectedItem != null)
            {
                MessageBox.Show("Il n'y a plus de bouteille de la marque sélectionnée dans la réserve.", "Manque de bouteille", MessageBoxButton.OK, MessageBoxImage.Warning);
                cboÉtiquette.IsEnabled = false;
            }
            if (FormulaireRempli())
                btnConfirmer.IsEnabled = true;
            else
                btnConfirmer.IsEnabled = false;
        }

      private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
            //Si un type est choisi, j'active la liste de marque
            if(cboMarqueBouteille.IsEnabled==false)
            cboMarqueBouteille.IsEnabled = true;

            lstMarques = PeuplerListeMarqueAlcool();
           

         
             cboMarqueBouteille.ItemsSource = lstMarques;
             cboMarqueBouteille.SelectedValuePath = "IdMarque";
             cboMarqueBouteille.DisplayMemberPath = "Nom";

            if (FormulaireRempli())
            {
                btnConfirmer.IsEnabled = true;
            }
            else
            {
                btnConfirmer.IsEnabled = false;
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            cboEmplacement.IsEnabled = true;
            btnConfirmer.IsEnabled = false;
        }

        private bool FormulaireRempli()
        {
            if (cboType.SelectedItem != null && cboMarqueBouteille.SelectedItem != null && cboÉtiquette.SelectedItem != null)
            {
                if (chbVendu.IsChecked == true && cboEmplacement.SelectedItem == null)
                {
                    return true;
                }
                else if (chbVendu.IsChecked == false && cboEmplacement.SelectedItem != null)
                {
                    return true;
                }
            }
            return false;            
        }

        private void cboÉtiquette_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FormulaireRempli())
            {
                btnConfirmer.IsEnabled = true;
            }
            else
            {
                btnConfirmer.IsEnabled = false;
            }
        }

        private void cboEmplacement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FormulaireRempli())
            {
                btnConfirmer.IsEnabled = true;
            }
            else
            {
                btnConfirmer.IsEnabled = false;
            }
        }

        private List<TypeAlcool> PeuplerListeTypeAlcool()
        {
            //Retrouve les id des marques des bouteilles en réserve
            lstIdMarquesEnReserve = new List<int?>(HibernateBouteilleService.RetrieveIdMarqueEnReserve());
            List<int?> lstIdTypeEnReserve = new List<int?>();

            //Pour chaque idMarque dans la liste, je trouve le type de cette marque, donc c'est les types d'alcool en réserve
            foreach (int? i in lstIdMarquesEnReserve)
            {
                
                lstIdTypeEnReserve.AddRange(HibernateMarqueService.RetrieveIdTypeEnReserve(i));
            }
            //Pour chaque idType dans la liste des types en réserve, je trouve le type d'alcool associé en BD
            foreach (int? i in lstIdTypeEnReserve)
            {
                List<TypeAlcool> leType = HibernateTypeAlcoolService.RetrieveTypeAlcoolById(i);
                //Si le type n'est pas déjà dans la liste des types, je l'ajoute
                if (lstType.Contains(leType[0]) == false)
                    lstType.Add(leType[0]);
            }

            return lstType;
        }
        private List<Marque> PeuplerListeMarqueAlcool()
        {
            

            lstMarques = new List<Marque>();
            List<Marque> lstMarqueDuType;

            //Je trouve tous les id des marques en réserve
            lstIdMarquesEnReserve = HibernateBouteilleService.RetrieveIdMarqueEnReserve();
            //Je trouve toutes les marques du type sélectionné
            lstMarqueDuType = HibernateMarqueService.RetrieveByType((TypeAlcool)cboType.SelectedItem);

            //Pour chaque Marque du type sélectionné, je vérifie si cette marque est en réserve grâce à la liste des idMarques en réserve
            // et si elle est déjà dans la liste de marques. Si ces conditions sont respecté, je l'ajoute dans la liste de marque
            foreach (Marque m in lstMarqueDuType)
            {
                if (lstIdMarquesEnReserve.Contains(m.IdMarque) && !lstMarques.Contains(m))
                {
                    lstMarques.Add(m);
                }
            }

            return lstMarques;
        }

        private void chbVendu_Checked(object sender, RoutedEventArgs e)
        {
            cboEmplacement.SelectedIndex = -1;
            cboEmplacement.IsEnabled = false;

            if (FormulaireRempli())
            {
                btnConfirmer.IsEnabled = true;
            }
            else
            {
                btnConfirmer.IsEnabled = false;
            }

        }
    }
}
