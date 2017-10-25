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
    /// Logique d'interaction pour EcranNouvelleCommande.xaml
    /// </summary>
    public partial class EcranNouvelleCommande : UserControl
    {
        private ObservableCollection<Marque> listMarque = new ObservableCollection<Marque>(ChargerListMarque());
        private List<Bouteille> lstBouteilles = new List<Bouteille>();
        private List<TypeAlcool> lstType = new List<TypeAlcool>(HibernateTypeAlcoolService.RetrieveAll());
        private List<Marque> lstMarques = new List<Marque>();
        public EcranNouvelleCommande()
        {
            InitializeComponent();


            cboMarqueBouteille.ItemsSource = listMarque;
            cboMarqueBouteille.DisplayMemberPath = "Nom";
            cboMarqueBouteille.SelectedValuePath = "IdMarque";

            cboFormat.Items.Add("26");
            cboFormat.Items.Add("40");
            cboFormat.IsEnabled = false;

            cboType.ItemsSource = lstType;
            cboType.DisplayMemberPath = "Nom";

            cboMarqueBouteille.IsEnabled = false;



        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(3);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(3);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        static private List<Marque> ChargerListMarque()
        {
            List<Marque> listM = new List<Marque>();
            listM = HibernateMarqueService.RetrieveAll();
            return listM;      
        }

        //private void cboMarque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Marque m = HibernateMarqueService.Retrieve((int)int.Parse(cboMarque.SelectedValue.ToString()))[0];
        //    m.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)m.IdTypeAlcool)[0];
        //    lblTypeAlcool.Content = m.SonTypeAlcool.Nom.ToString();

            
        //}

        private void cboMarqueBouteille_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (cboMarqueBouteille.SelectedItem != null)
                lstBouteilles = HibernateBouteilleService.RetrieveByMarque((Marque)cboMarqueBouteille.SelectedItem);
            cboFormat.IsEnabled = true;

        }

        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                cboMarqueBouteille.IsEnabled = true;

            lstMarques = HibernateMarqueService.RetrieveByType((TypeAlcool)cboType.SelectedItem);

            cboMarqueBouteille.ItemsSource = lstMarques;
            cboMarqueBouteille.SelectedValuePath = "IdMarque";
            cboMarqueBouteille.DisplayMemberPath = "Nom";

        }

        private void cboFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Bouteille> listB = new List<Bouteille>();
            listB = HibernateBouteilleService.RetrieveByMarqueEtVolInitial((int)int.Parse(cboMarqueBouteille.SelectedValue.ToString()), int.Parse(cboFormat.SelectedValue.ToString()));
            Random r = new Random();
            r.Next(20, 70);
            if (listB.Count > 0)
            {
                if (listB.ElementAt(0).PrixBouteille != null)
                {
                    lblPrix.Content = listB.ElementAt(0).PrixBouteille.ToString();
                }
                
            }
            else
            {

                lblPrix.Content = r.ToString();
            }
        }
    }
}
