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
    /// Logique d'interaction pour EcranConsulterVente.xaml
    /// </summary>
    public partial class EcranConsulterVente : UserControl
    {
        private ObservableCollection<Vente> lstVente;
       
        public EcranConsulterVente()
        {
            InitializeComponent();
           
            
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(2);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

       

        private void cldVente_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? d = cldVente.SelectedDate;
            List<Vente> LalistPourCollection = new List<Vente>();

            LalistPourCollection = HibernateVenteService.RetrieveVenteEmploye((int)EcranAccueil.employe.IdEmploye, (DateTime)d);
            lstVente = new ObservableCollection<Vente>(LalistPourCollection);
            dtgVenteEmploye.ItemsSource = lstVente;

        }
    }
}
