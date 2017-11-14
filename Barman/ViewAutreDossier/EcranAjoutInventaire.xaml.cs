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
using Barman.MarqueDossier;
using Barman.MarqueDossier.Hibernate;
using Barman.TypeDossier;
using Barman.TypeDossier.Hibernate;

namespace Barman.ViewAutreDossier
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




        }







        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(0);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
            btnConfirmer.IsEnabled = false;
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {


            
            if (txtNouveauType.Text != "" || txtMarque.Text != "")
            {
                if (txtNouveauType.Text != "")                
                    btnAjouterNouveauTypeA_Click(sender, e);               
                if (txtMarque.Text != "")                
                    btnAjouterNouvelleM_Click(sender, e);
                if (txtNouveauType.Text == "" || txtMarque.Text == "")
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                    EcranOnglets EO = new EcranOnglets(0);
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
                    btnAnnuler.IsEnabled = false;
                }
            }
            else
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
                EcranOnglets EO = new EcranOnglets(0);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
                btnAnnuler.IsEnabled = false;
            }

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



        private void btnAjouterNouvelleM_Click(object sender, RoutedEventArgs e)
        {
            if (ValideMarqueAlcool())
            {
                HibernateMarqueService.Create(new Marque(txtMarque.Text, int.Parse(cboTypeAlcool.SelectedValue.ToString())));
                txtMarque.Clear();
            }
            else
            {
                lblInfoMessage.Foreground = Brushes.Red;
                lblInfoMessage.Content = "Une erreur est survenue. Passé votre souris sur le rouge pour en connaitre la cause.";
            }

        }

        private bool ValideMarqueAlcool()
        {
            bool estValide = true;
            List<string> lstNomMarque = new List<string>(HibernateMarqueService.RetrieveAllNomMarque());

            if (txtMarque.Text.Length > 100 )
            {
                estValide = false;
                txtMarque.ToolTip = "Le nom de la nouvelle marque d'alcool entrée est trop long";
                txtMarque.BorderBrush = Brushes.Red;
            }
            if (!TypeDissponible(lstNomMarque, txtMarque.Text))
            {
                estValide = false;
                txtMarque.BorderBrush = Brushes.Red;
                txtMarque.ToolTip = "Le nom de la nouvelle marque d'alcool entrée existe déjà dans la base de données";
            }
            if (txtMarque.Text.Length == 0)
            {
                estValide = false;
                txtMarque.BorderBrush = Brushes.Red;
                txtMarque.ToolTip = "le champ d'entré pour la nouvelle marque est vide";
            }


            return estValide;
        }

        private void btnAjouterNouveauTypeA_Click(object sender, RoutedEventArgs e)
        {
            if (ValidetypeAlcool())
            {
                HibernateTypeAlcoolService.Create(new TypeAlcool(txtNouveauType.Text));
                txtNouveauType.Clear();

                cboTypeAlcool.ItemsSource = new ObservableCollection<TypeAlcool>(ChargerListTypeAlcool());
            }
            else
            {
                lblInfoMessage.Foreground = Brushes.Red;
                lblInfoMessage.Content = "Une erreur est survenue. Passé votre souris sur le rouge pour en connaitre la cause.";
            }
        }

        private bool ValidetypeAlcool()
        {
            List<string> lstTypeAlcool = new List<string>(HibernateTypeAlcoolService.RetrieveAllTypeAlcool());
            bool estValide = true;

            if (txtNouveauType.Text.Length > 50)
            {
                estValide = false;
                txtNouveauType.ToolTip = "Le nouveau type d'alcool entré est trop long pour être enregistré";
                txtNouveauType.BorderBrush = Brushes.Red;
            }
            if (txtNouveauType.Text.Length == 0)
            {
                estValide = false;
                txtNouveauType.ToolTip = "le champ du nouveau type d'alcool est vide";
                txtNouveauType.BorderBrush = Brushes.Red;
            }
            if (!TypeDissponible(lstTypeAlcool, txtNouveauType.Text))
            {
                estValide = false;
                txtNouveauType.ToolTip = "Le nouveau type d'alcool entré existe déjà dans la base de données";
                txtNouveauType.BorderBrush = Brushes.Red;
            }

            return estValide;
        }

        private bool TypeDissponible(List<string> t, string text)
        {
            bool estValide = true;        

            foreach (var nom in t)
            {
                if (nom.Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    estValide = false;
                }
            }
            return estValide;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
