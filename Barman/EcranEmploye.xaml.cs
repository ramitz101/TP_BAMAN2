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
    /// Logique d'interaction pour EcranEmploye.xaml
    /// </summary>
    /// 

        // dfasdfasdf
    public partial class EcranEmploye : UserControl
    {

        private static ObservableCollection<Employe> lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye());

        public EcranEmploye()
        {
            InitializeComponent();
            dtgEmploye.ItemsSource = lstEmployes;
            

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
            foreach(var i in listE)
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
    }
}
