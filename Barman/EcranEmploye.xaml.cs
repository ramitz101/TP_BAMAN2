using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logique d'interaction pour EcranEmploye.xaml
    /// </summary>
    /// 

    // dfasdfasdf
    public partial class EcranEmploye : UserControl
    {
        public string ContenuHeader { get; set; }

        private static ObservableCollection<Employe> lstEmployes = new ObservableCollection<Employe>();

        public EcranEmploye()
        {
            InitializeComponent();
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye());
            dtgEmploye.ItemsSource = lstEmployes;
            dtgEmploye.IsReadOnly = true;
            ContenuHeader = "Nom";

        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (UnEmployeSelectionne())
            {
                FenetreModifierEmploye popup = new FenetreModifierEmploye(lstEmployes, dtgEmploye.SelectedItem as Employe, this);
                popup.ShowDialog();

            }

            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
            //dtgEmploye.Items.Refresh();
        }

        private void btnAjouterEmploye_Click(object sender, RoutedEventArgs e)
        {
            FenetreAjouterEmploye popup = new FenetreAjouterEmploye();
            popup.ShowDialog();

            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
            //dtgEmploye.Items.Refresh();
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        private static List<Employe> ChargerListEmploye()
        {
            List<Employe> listE = new List<Employe>(HibernateEmployeService.RetrieveAll());
            foreach (var i in listE)
            {
                i.SonRole = HibernateRoleService.Retrieve((int)i.IdRole)[0];
            }
            return listE;
        }

        private bool UnEmployeSelectionne()
        {
            if (dtgEmploye.SelectedItems.Count == 1)
                return true;
            // si le user a sélectionné plus d'une inscription 
            else if (dtgEmploye.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vous avez trops d'employé selectionnées");
                return false;
            }
            else
            {
                MessageBox.Show("Vous devez selectionner un employé");
                return false;
            }
        }

        private void btnSuppression_Click(object sender, RoutedEventArgs e)
        {
            if (AuMoinsUnEmployeSelectionne())
            {

                MessageBoxResult resultat = MessageBox.Show("Êtes vous sûr de vouloire supprimer la sélection d'employés?", "Question", MessageBoxButton.YesNo);

                if (resultat == MessageBoxResult.Yes)
                {
                    List<Employe> lstEmploye = dtgEmploye.SelectedItems.Cast<Employe>().ToList();

                    foreach (var employe in lstEmploye)
                    {
                        HibernateEmployeService.Delete(employe);
                    }
                    dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
                }

            }
        }

        private bool AuMoinsUnEmployeSelectionne()
        {
            if (dtgEmploye.SelectedItems.Count > 0)
                return true;
            else
            {
                MessageBox.Show("Vous devez selectionner un employe");
                return false;
            }
        }

        private void txtRecherche_KeyUp_1(object sender, KeyEventArgs e)
        {
            List<Employe> lstEmploye = new List<Employe>();
            List<Role> lstRole = new List<Role>();
            if (txtRecherche.Text != "" && txtRecherche.Text != "Recherche")
            {
                switch (ContenuHeader)
                {
                    case "Prénom":
                        lstEmploye = HibernateEmployeService.RetrievePrenom(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;

                        break;
                    case "Téléphone":
                        lstEmploye = HibernateEmployeService.RetrieveTelephone(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Date d'embauche":
                        lstEmploye = HibernateEmployeService.RetrieveDateEmbauche(txtRecherche.Text);
                        
                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "NAS":
                        lstEmploye = HibernateEmployeService.RetrieveNAS(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Role":
                        lstRole = HibernateRoleService.RetrieveRole(txtRecherche.Text);
                        foreach (Role r in lstRole)
                        {
                            lstEmploye.AddRange(HibernateEmployeService.RetrieveRole((int)r.IdRole));
                        }

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Code":
                        lstEmploye = HibernateEmployeService.RetrieveCode(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    default:
                        lstEmploye = HibernateEmployeService.RetrieveNom(txtRecherche.Text);
                        /*foreach (Employe employe in lstEmploye)
                        {
                            lstEmploye.AddRange(HibernateEmployeService.RetrieveByMarqueId((int)m.IdMarque));
                        }*/

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else                        
                            dtgEmploye.ItemsSource = null;                        
                        break;
                }
            }
            else
            {
                lstEmploye = HibernateEmployeService.RetrieveAll();
                dtgEmploye.ItemsSource = lstEmploye;
            }
        }

        private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            //List<Employe> lstEmploye = HibernateEmployeService.RetrieveAll();
            /*foreach (Bouteille b in lstBouteille)
            {
                b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
            }*/
            //dtgEmploye.ItemsSource = lstEmploye;
        }

        private void txtRecherche_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "Rechercher";
        }

        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            ContenuHeader = ((DataGridColumnHeader)sender).Content.ToString();
        }
    }
}
