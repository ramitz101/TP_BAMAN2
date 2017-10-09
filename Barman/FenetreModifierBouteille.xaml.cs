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
using System.Windows.Shapes;

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour FenetreModifierBouteille.xaml
    /// </summary>
    public partial class FenetreModifierBouteille : Window
    {
        private Bouteille bouteilleModifier;
        private static ObservableCollection<Emplacement> lstEmplacements = new ObservableCollection<Emplacement>(ChargerListEmplacement());
        public FenetreModifierBouteille(ObservableCollection<Bouteille> lstBouteille, Bouteille bouteille, EcranInventaire ecranInventaire)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;

            if (bouteille != null)
            {
                bouteilleModifier = bouteille;
                lblMarque.Content = bouteille.SaMarque.Nom;
                lblFormatBouteille.Content = bouteille.VolumeInitial.ToString();
                txtVolumeRestant.Text = bouteille.VolumeRestant.ToString();
                cboEmplacement.ItemsSource = lstEmplacements;
                cboEmplacement.DisplayMemberPath = "Nom";
                cboEmplacement.SelectedValuePath = "IdEmplacement";
                cboEmplacement.SelectedValue = bouteille.SonEmplacement.IdEmplacement;
                

            }


        }

        private static List<Emplacement> ChargerListEmplacement()
        {
            List<Emplacement> listE = new List<Emplacement>(HibernateEmplacementService.RetrieveAll());
            return listE;
        }



        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
