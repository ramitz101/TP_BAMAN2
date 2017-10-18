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
    /// Logique d'interaction pour EcranGererVente.xaml
    /// </summary>
    public partial class EcranGererVente : UserControl
    {
        private ObservableCollection<Employe> lstEmploye = new ObservableCollection<Employe>(ChargerListEmploye());
        private ObservableCollection<Vente> lstVente;

        public EcranGererVente()
        {
            InitializeComponent();

            cboEmploye.ItemsSource = lstEmploye;
            cboEmploye.DisplayMemberPath = "Nom";
            cboEmploye.SelectedValuePath = "IdEmploye";
            cboEmploye.SelectedIndex = 0;
            
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(2);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {

        }

        private static List<Employe> ChargerListEmploye()
        {
            List<Employe> listE = new List<Employe>(HibernateEmployeService.RetrieveAll());
            return listE;
        }
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            // attente du binding
        }

        private void cboEmploye_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? d = cldVente.SelectedDate;
            List<Vente> LalistPourCollection = new List<Vente>();

            if(d == null)
                d = DateTime.Today;

            LalistPourCollection = HibernateVenteService.RetrieveVenteEmploye((int)cboEmploye.SelectedValue, (DateTime)d);
            lstVente = new ObservableCollection<Vente>(LalistPourCollection);
            dtgVenteEmploye.ItemsSource = lstVente;


        }

        private void cldVente_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? d = cldVente.SelectedDate;
            List<Vente> LalistPourCollection = new List<Vente>();

            LalistPourCollection = HibernateVenteService.RetrieveVenteEmploye((int)cboEmploye.SelectedValue, (DateTime)d);
            lstVente = new ObservableCollection<Vente>(LalistPourCollection);
            dtgVenteEmploye.ItemsSource = lstVente;

        }
    }
}
