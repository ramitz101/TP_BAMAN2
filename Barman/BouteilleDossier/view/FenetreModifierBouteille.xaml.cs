﻿using System;
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
                List<int> lstVolumes = new List<int>();
                for (int i = 1; i <= bouteille.VolumeInitial; i++)
                {
                    lstVolumes.Add(i);
                }

                cboVolumeRestant.ItemsSource = lstVolumes;

                cboVolumeRestant.SelectedValue = bouteille.VolumeRestant;
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
            btnConfirmer.IsEnabled = false;
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {

            bouteilleModifier.VolumeRestant = (int)cboVolumeRestant.SelectedValue;
            if (bouteilleModifier.VolumeRestant < bouteilleModifier.VolumeInitial)
            {
                bouteilleModifier.Etat = "Entamée";
            }
            bouteilleModifier.IdEmplacement = (int)cboEmplacement.SelectedValue;
            HibernateBouteilleService.Update(bouteilleModifier);
            this.Close();

        }

        private void btnPerte_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show("La bouteille sera condidérée comme perdue. Est-ce bien ce que vous voulez faire?", "Question", MessageBoxButton.YesNo);

            if (resultat == MessageBoxResult.Yes)
            {

                bouteilleModifier.Etat = "Perdue";
                bouteilleModifier.IdEmplacement = HibernateEmplacementService.retrieveEmplacementByNom("Aucun")[0].IdEmplacement;
                HibernateBouteilleService.Update(bouteilleModifier);
                this.Close();
            }
        }
    }
}
