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
            FenetreModifierEmploye popup = new FenetreModifierEmploye();
            popup.ShowDialog();
        }

        private void btnAjouterEmploye_Click(object sender, RoutedEventArgs e)
        {
            FenetreAjouterEmploye popup = new FenetreAjouterEmploye();
            popup.ShowDialog();
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
            return listE;
        }
    }
}
