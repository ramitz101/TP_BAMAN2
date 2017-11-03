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
using Barman.BouteilleDossier.Hibernate;
using Barman.EmplacementDossier;
using Barman.EmplacementDossier.Hibernate;

namespace Barman.BouteilleDossier.view
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
                txtMarque.Content = bouteille.SaMarque.Nom;
                txtFormatBouteille.Content = bouteille.VolumeInitial.ToString();
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
            string message = "Modification effectuée avec succès. Vous avez modifié ";
            if (ValideChamp())
            {
                if (bouteilleModifier.VolumeRestant != int.Parse(txtVolumeRestant.Text))
                {
                    message += "le volume";
                    bouteilleModifier.VolumeRestant = int.Parse(txtVolumeRestant.Text);
                }
                if (bouteilleModifier.IdEmplacement != (int)cboEmplacement.SelectedValue)
                {
                    bouteilleModifier.IdEmplacement = (int)cboEmplacement.SelectedValue;
                    if(message.EndsWith(" "))
                        message += "l'emplacement";
                    else
                        message += " l'emplacement";

                }
                message += ".";
                HibernateBouteilleService.Update(bouteilleModifier);
                MessageBox.Show(message, "Modification", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            else
            {
                MessageBox.Show("Erreur dans la validation des champs modifier");
            }

        }

        private bool ValideChamp()
        {
            bool estValide = false;
            int n;
            if (int.TryParse(txtVolumeRestant.Text, out n))
            {
                if (n <= int.Parse(txtFormatBouteille.Content.ToString()) && n>0)
                {
                    estValide = true;
                }
            }
            
            return estValide;
        }
    }
}
