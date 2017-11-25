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
using Barman.BouteilleDossier;
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
        private List<Employe> lstEmployes = new List<Employe>();
        private List<int?> lstIdMarquesEnReserve = new List<int?>();
        private List<int?> lstIdTypeEnReserve = new List<int?>();

        public EcranFormulaireBouteille()
        {
            InitializeComponent();

            if (EcranAccueil.Employe.SonRole.Code == Constante.UTILISATEUR)
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Utilisateur"+" - Sortir une bouteille de la réserve";
            else
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Administrateur" + " - Sortir une bouteille de la réserve";

            lstIdMarquesEnReserve = new List<int?>(HibernateBouteilleService.RetrieveIdMarqueEnReserve());
            lstIdTypeEnReserve = new List<int?>();

            foreach (int? i in lstIdMarquesEnReserve)
            {
                lstIdTypeEnReserve.AddRange(HibernateMarqueService.RetrieveIdTypeEnReserve(i));
            }
            foreach (int? i in lstIdTypeEnReserve)
            {
                List<TypeAlcool> leType = HibernateTypeAlcoolService.RetrieveTypeAlcoolById(i);
                if (lstType.Contains(leType[0]) == false)
                    lstType.Add(leType[0]);
            }


            cboType.ItemsSource = lstType;
            cboType.DisplayMemberPath = "Nom";



            cboEmplacement.ItemsSource = lstEmplacements;
            cboEmplacement.DisplayMemberPath = "Nom";
            //lstEmployes = HibernateEmployeService.Retrieve(EcranAccueil.employe.IdEmploye);
            lblEmployeConnecte.Content = EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom;
        


            lstEmployes = HibernateEmployeService.Retrieve(EcranAccueil.Employe.IdEmploye);
            lblEmployeConnecte.Content = EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom;

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
            if (FormulaireRempli())
            {

                Bouteille bouteilleAChanger = HibernateBouteilleService.RetrieveByUnique((int)HibernateMarqueService.Retrieve(cboMarqueBouteille.Text)[0].IdMarque, (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement, cboÉtiquette.Text)[0];
                if (chbVendu.IsChecked == false)
                {
                    bouteilleAChanger.IdEmplacement = (int)HibernateEmplacementService.retrieveEmplacementByNom(cboEmplacement.Text)[0].IdEmplacement;
                }
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
            {
                MessageBox.Show("Refusé");
            }
        }

        private void cboMarqueBouteille_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboÉtiquette.IsEnabled = true;
            if (cboMarqueBouteille.SelectedItem != null)
                lstBouteilles = HibernateBouteilleService.RetrieveByMarque((Marque)cboMarqueBouteille.SelectedItem);
            cboÉtiquette.ItemsSource = lstBouteilles;
            cboÉtiquette.DisplayMemberPath = "Numero";

            if (cboÉtiquette.Items.Count == 0 && cboMarqueBouteille.SelectedItem != null)
            {
                MessageBox.Show("Il n'y a plus de bouteille de la marque sélectionnée dans la réserve.", "Manque de bouteille", MessageBoxButton.OK, MessageBoxImage.Warning);
                cboÉtiquette.IsEnabled = false;
            }

            if (FormulaireRempli())
            {
                btnConfirmer.IsEnabled = true;
            }
            else
            {
                btnConfirmer.IsEnabled = false;
            }

        }

        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboMarqueBouteille.IsEnabled == false)
                cboMarqueBouteille.IsEnabled = true;

            lstMarques = new List<Marque>();

            List<Marque> lstMarqueDuType;

            lstIdMarquesEnReserve = HibernateBouteilleService.RetrieveIdMarqueEnReserve();
            lstMarqueDuType = HibernateMarqueService.RetrieveByType((TypeAlcool)cboType.SelectedItem);

            foreach (Marque m in lstMarqueDuType)
            {
                if (lstIdMarquesEnReserve.Contains(m.IdMarque) && !lstMarques.Contains(m))
                {
                    lstMarques.Add(m);
                }
            }


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

        private void chbVendu_Unchecked(object sender, RoutedEventArgs e)
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


    }
}
