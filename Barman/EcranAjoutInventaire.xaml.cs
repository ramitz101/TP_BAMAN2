using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour EcranAjoutInventaire.xaml
    /// </summary>
    public partial class EcranAjoutInventaire : UserControl
    {

        //private ObservableCollection<Bouteille> lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille());
        private ObservableCollection<Marque> lstMarques = new ObservableCollection<Marque>(ChargerListMarque());
        private ObservableCollection<TypeAlcool> lstTypeAlcool = new ObservableCollection<TypeAlcool>(ChargerListTypeAlcool());

        

        public EcranAjoutInventaire()
        {
            InitializeComponent();


            cboTypeAlcool.ItemsSource = lstTypeAlcool;
            cboTypeAlcool.DisplayMemberPath = "Nom";
            cboTypeAlcool.SelectedValuePath = "IdTypeAlcool";
            cboTypeAlcool.SelectedIndex = 0;

            cboMarque.ItemsSource = lstMarques;                       
            cboMarque.DisplayMemberPath = "Nom";
            cboMarque.SelectedValuePath = "IdMarque";
            cboMarque.SelectedIndex = 0;


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
        }

        private void btnAugmenterQ_Click(object sender, RoutedEventArgs e)
        {
            int quantite;
            try
            {
                quantite = Int32.Parse(txtQuantite.Text);
                quantite++;
                txtQuantite.Text = quantite.ToString();
            }
            catch (Exception ex)
            {
                quantite = 1;
                txtQuantite.Text = quantite.ToString();
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(0);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {

            
            //lstBouteilles.Add(new Bouteille(cboMarque.SelectedIndex, int.Parse(txtVolume.Text)));

            
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(0);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        

        private static List<Marque> ChargerListMarque()
        {
            List<Marque> listM = new List<Marque>(HibernateMarqueService.RetrieveAll());
            return listM;
        }

        private static List<TypeAlcool> ChargerListTypeAlcool()
        {
            List<TypeAlcool> listTA = new List<TypeAlcool>(HibernateTypeAlcoolService.RetrieveAll());
            return listTA;
        }

        private void btnAjouterCommande_Click(object sender, RoutedEventArgs e)
        {
            int quantite = int.Parse(txtQuantite.Text);

            for (int i = 0; i < quantite; i++)
            {
                HibernateBouteilleService.Create(new Bouteille(int.Parse(cboMarque.SelectedValuePath), int.Parse(txtVolume.Text), txtNumero.Text, float.Parse(txtPrix.Text, CultureInfo.InvariantCulture.NumberFormat)));            
            }

            txtVolume.Clear();
         
            txtQuantite.Text = "1";
        }

        private void btnAjouterNouvelleM_Click(object sender, RoutedEventArgs e)
        {
            if (ValideMarqueAlcool())
            {           
                HibernateMarqueService.Create(new Marque(txtMarque.Text, int.Parse(cboTypeAlcool.SelectedValue.ToString())));
                txtMarque.Clear();
            }
            else
            {
                MessageBox.Show("Le nom de la marque d'alcool n'est pas valide");
            }
            
        }

        private bool ValideMarqueAlcool()
        {
            bool estValide = true;
            List<string> lstNomMarque = new List<string>(HibernateMarqueService.RetrieveAllNomMarque());

            if (txtMarque.Text.Length < 100)
            {
                foreach (var nom in lstNomMarque)
                {
                    if (nom == txtMarque.Text)
                    {
                        estValide = false;
                    }
                }
            }
            else
            {
                estValide = false;
            }

            return estValide;
        }

        private void btnAjouterNouveauTypeA_Click(object sender, RoutedEventArgs e)
        {
            if (ValidetypeAlcool())
            {
                HibernateTypeAlcoolService.Create(new TypeAlcool(txtNouveauType.Text));
                txtNouveauType.Clear();
            }
            else
            {
                MessageBox.Show("Le type d'alcool entré n'est pas valide");
            }
            
        }

        private bool ValidetypeAlcool()
        {
            bool estValide = true;
            Regex r = new Regex("^[a-zA-Z]*$");            
            List<string> lstTypeAlcool = new List<string>(HibernateTypeAlcoolService.RetrieveAllTypeAlcool());

            if (txtNouveauType.Text.Length < 50 && r.IsMatch(txtNouveauType.Text))
            {
                foreach (var nom in lstTypeAlcool)
                {
                    if (nom == txtNouveauType.Text)
                    {
                        estValide = false;
                    }

                }
            }
            else
            {
                estValide = false;
            }
            return estValide;
        }
    }
}
