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
        private List<Bouteille> lstNouvelleBouteille = new List<Bouteille>();
        private Commande CommandeCours;
        public EcranNouvelleCommande()
        {
            InitializeComponent();


            cboMarqueBouteille.ItemsSource = listMarque;
            cboMarqueBouteille.DisplayMemberPath = "Nom";
            cboMarqueBouteille.SelectedValuePath = "IdMarque";

       

            cboType.ItemsSource = lstType;
            cboType.DisplayMemberPath = "Nom";

            cboMarqueBouteille.IsEnabled = false;

            CommandeCours = new Commande(DateTime.Now, (int)EcranAccueil.employe.IdEmploye, null, "Envoyé");
            HibernateCommandeService.Create(CommandeCours);

        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            HibernateCommandeService.Delete(CommandeCours);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranCommande EC = new EcranCommande();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EC);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var i in lstNouvelleBouteille)
                {
                    HibernateBouteilleService.Create(i);
                }
                MessageBox.Show("La commande a bien été envoyé", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranCommande EC = new EcranCommande();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EC);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Une erreur c'est produite ", "Attention", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
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
            if(txtFormat.Text != "")
                RefreshLabelPrix();

        }

        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                cboMarqueBouteille.IsEnabled = true;

            lstMarques = HibernateMarqueService.RetrieveByType((TypeAlcool)cboType.SelectedItem);

            cboMarqueBouteille.ItemsSource = lstMarques;
            cboMarqueBouteille.SelectedValuePath = "IdMarque";
            cboMarqueBouteille.DisplayMemberPath = "Nom";

        }

       

        private void txtFormat_KeyDown(object sender, KeyEventArgs e)
        {
            RefreshLabelPrix();
            
        }

        private void RefreshLabelPrix()
        {
            lblPrix.Content = "";
            List<Bouteille> listB = new List<Bouteille>();
            Random r = new Random();
            int prix = r.Next(20, 70);
            try
            {
                listB = HibernateBouteilleService.RetrieveByMarqueEtVolInitial((int)int.Parse(cboMarqueBouteille.SelectedValue.ToString()), int.Parse(txtFormat.Text));
            }
            catch (Exception ex) { }

            if (listB.Count > 0)
            {
                if (listB.ElementAt(0).PrixBouteille != null)
                {
                    lblPrix.Content = listB.ElementAt(0).PrixBouteille.ToString();
                }

            }
            else
            {

                lblPrix.Content = prix.ToString();
            }
        }

        private void btnAugmenteQ_Click(object sender, RoutedEventArgs e)
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

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {

            for(int i =0; i < int.Parse(txtQuantite.Text);i++)
            {
                Bouteille b = new Bouteille(Generer.GenererCodeBouteille(), int.Parse(txtFormat.Text), int.Parse(txtFormat.Text), "Pleine", float.Parse(lblPrix.Content.ToString()),
                                            9, int.Parse(cboMarqueBouteille.SelectedValue.ToString()), (int)CommandeCours.IdCommande);
                b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                lstNouvelleBouteille.Add(b);
            }

            dtgNouvelleCommande.ItemsSource = lstNouvelleBouteille;
            dtgNouvelleCommande.Items.Refresh();
        }
    }
}
