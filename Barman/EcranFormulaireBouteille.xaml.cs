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
    /// Logique d'interaction pour EcranFormulaireBouteille.xaml
    /// </summary>
    public partial class EcranFormulaireBouteille : UserControl
    {
      private List<Bouteille> lstBouteilles = new List<Bouteille>();
      private List<TypeAlcool> lstType = new List<TypeAlcool>(HibernateTypeAlcoolService.RetrieveAll());
      private List<Emplacement> lstEmplacements = new List<Emplacement>(HibernateEmplacementService.RetrieveAllForFormulaire());
      private List<Marque> lstMarques = new List<Marque>();
      private List<Employe> lstEmployes = new List<Employe>(HibernateEmployeService.RetrieveAll());
      public EcranFormulaireBouteille()
      {
         InitializeComponent();
         

         cboType.ItemsSource = lstType;
         cboType.DisplayMemberPath = "Nom";

         

         cboEmplacement.ItemsSource = lstEmplacements;
         cboEmplacement.DisplayMemberPath = "Nom";

         cboEmployé.ItemsSource = lstEmployes;
         cboEmployé.DisplayMemberPath = "Nom";

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
            if(FormulaireRempli())
            {
               
               Bouteille bouteilleAChanger = HibernateBouteilleService.RetrieveByUnique((int)HibernateMarqueService.Retrieve(cboMarqueBouteille.Text)[0].IdMarque, (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement, cboÉtiquette.Text)[0];
               if(chbVendu.IsChecked==false)
                  bouteilleAChanger.IdEmplacement = (int)HibernateEmplacementService.retrieveEmplacementByNom(cboEmplacement.Text)[0].IdEmplacement;
               else
                  bouteilleAChanger.IdEmplacement=(int)HibernateEmplacementService.retrieveEmplacementByNom("Aucun")[0].IdEmplacement;

               HibernateBouteilleService.Update(bouteilleAChanger);
               
               MessageBox.Show("Modification éffectuée avec succès!", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Information);

               ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
               ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, new EcranFormulaireBouteille());

            }
            else
            {
               MessageBox.Show("Refusé");
            }
        }

      private void cboMarqueBouteille_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         cboÉtiquette.IsEnabled = true;
         if(cboMarqueBouteille.SelectedItem!=null)
            lstBouteilles = HibernateBouteilleService.RetrieveByMarque((Marque)cboMarqueBouteille.SelectedItem);
         cboÉtiquette.ItemsSource = lstBouteilles;
         cboÉtiquette.DisplayMemberPath = "Numero";

      }

      private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if(cboMarqueBouteille.IsEnabled==false)
         cboMarqueBouteille.IsEnabled = true;
        
         lstMarques = HibernateMarqueService.RetrieveByType((TypeAlcool)cboType.SelectedItem);
         
         cboMarqueBouteille.ItemsSource = lstMarques;
         cboMarqueBouteille.SelectedValuePath = "IdMarque";
         cboMarqueBouteille.DisplayMemberPath = "Nom";

      }

      private void CheckBox_Checked(object sender, RoutedEventArgs e)
      {
         cboEmplacement.SelectedIndex = -1;
         cboEmplacement.IsEnabled = false;

      }

      private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
      {
         cboEmplacement.IsEnabled = true;
      }

      private bool FormulaireRempli()
      {
         if (cboType.SelectedItem != null&&cboMarqueBouteille.SelectedItem!=null&&cboEmployé.SelectedItem != null&&cboÉtiquette.SelectedItem != null)
         {
            if(chbVendu.IsChecked==true&&cboEmplacement.SelectedItem==null)
            {
               return true;
            }
            else if(chbVendu.IsChecked==false&&cboEmplacement.SelectedItem!=null)
            {
               return true;
            }
         }
         return false;
          

      }
   }
}
