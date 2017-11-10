using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using Barman.BouteilleDossier.Hibernate;
using Barman.EmplacementDossier.Hibernate;
using Barman.MarqueDossier;
using Barman.MarqueDossier.Hibernate;
using Barman.TypeDossier.Hibernate;
using Barman.ViewAutreDossier;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using Barman.EmplacementDossier;
using Barman.TypeDossier;
using System.Windows.Threading;
using System.Threading;

namespace Barman.BouteilleDossier.view
{
    /// <summary>
    /// Logique d'interaction pour inventaire.xaml
    /// </summary>
    public partial class EcranInventaire : UserControl
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private static ObservableCollection<Bouteille> lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille(false));
        public string ContenuHeader { get; set; }
        public EcranInventaire()
        {
            InitializeComponent();

            

            lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille(false));
            dtgInventaire.ItemsSource = lstBouteilles;
            dtgInventaire.Items.Refresh();
            ContenuHeader = "Nom";
            dtgInventaire.IsReadOnly = true;
            lblNbEntamee.Content += HibernateBouteilleService.CountNbEntamee().ToString();
            lblNbPlein.Content += HibernateBouteilleService.CountNbPleine().ToString();
            lblNbReserve.Content += HibernateBouteilleService.CountNbReserve().ToString();
            lblNbSupprimee.Content += HibernateBouteilleService.CountNbSupprimee().ToString();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            CommandManager.InvalidateRequerySuggested();
            if(dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();
            }
        }

        private void btnAjout_Click(object sender, RoutedEventArgs e)
        {
            if (EcranAccueil.employe.IdRole == null)
            {
                FenetreAuthentification FN = new FenetreAuthentification();
                FN.ShowDialog();
            }
            if (EcranAccueil.employe.IdRole != 1 && EcranAccueil.employe.IdRole != null)
            {
                FenetreErreur FE = new FenetreErreur();
                 FE.ShowDialog();
               

            }
            if (EcranAccueil.employe.IdRole == 1)
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranAjoutInventaire EAI = new EcranAjoutInventaire();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EAI);
            }

            dtgInventaire.ItemsSource = new ObservableCollection<Bouteille>(ChargerListBouteille(false));
        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (UneBouteilleSelectionne())
            {

                if (EcranAccueil.employe.IdRole == null)
                {
                    FenetreAuthentification FN = new FenetreAuthentification();
                    FN.ShowDialog();
                }
                if (EcranAccueil.employe.IdRole != 1 && EcranAccueil.employe.IdRole != null)
                {
                    FenetreErreur FE = new FenetreErreur();
                    FE.ShowDialog();
                }
                if (EcranAccueil.employe.IdRole == 1)
                {
                    FenetreModifierBouteille popup = new FenetreModifierBouteille(lstBouteilles, dtgInventaire.SelectedItem as Bouteille, this);
                    popup.ShowDialog();
                    if(popup.btnConfirmer.IsEnabled)
                    {
                        lblMessage.Text = "Bouteille modifié avec succès.";
                        lblMessage.Foreground = Brushes.Green;
                        
                    }
                    else if(popup.btnAnnuler.IsEnabled)
                    {
                        lblMessage.Text = "Modification de bouteille annulée.";
                        lblMessage.Foreground = Brushes.Red;
                    }
                }

                dtgInventaire.ItemsSource = new ObservableCollection<Bouteille>(ChargerListBouteille(false));
                //dtgInventaire.Items.Refresh();

            }            
        }
        
        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }


        // fonction charger tous les bouteille de l'inventaire (chargerListeBouteille) elle retourne la liste de la BD
        private static List<Bouteille> ChargerListBouteille(bool? sup)
        {
            List<Bouteille> listB = new List<Bouteille>(HibernateBouteilleService.RetrieveAll(sup));
            foreach (Bouteille b in listB)
            {
                b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];

            }

            return listB;
        }
        
       



        private bool UneBouteilleSelectionne()
        {
            if (dtgInventaire.SelectedItems.Count == 1)
                return true;
            // si le user a sélectionné plus d'une bouteille
            else if (dtgInventaire.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vous avez trops de bouteilles selectionnées");
                return false;
            }
            else
            {
                MessageBox.Show("Vous devez selectionner une bouteille");
                return false;
            }
        }

        private bool AuMoinsUneBouteilleSelectionne()
        {
            if (dtgInventaire.SelectedItems.Count > 0)
                return true;
            else
            {
                MessageBox.Show("Vous devez selectionner une bouteille");
                return false;
            }
        }

        private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            List<Bouteille> lstBouteille = HibernateBouteilleService.RetrieveAll(chbAfficherSupprimee.IsChecked);
            /*foreach (Bouteille b in lstBouteille)
            {
                b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
            }*/
            dtgInventaire.ItemsSource = lstBouteille;
        }

        private void txtRecherche_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "Rechercher";
        }




        private void txtRecherche_KeyUp_1(object sender, KeyEventArgs e)
        {
            List<Bouteille> lstBouteille = new List<Bouteille>();
            List<Marque> lstMarque = new List<Marque>();
            List<TypeAlcool> lstType = new List<TypeAlcool>();
            List<Emplacement> lstEmplacement= new List<Emplacement>();
            if (txtRecherche.Text != "" && txtRecherche.Text != "Recherche" )
            {
                switch (ContenuHeader)
                {
                    case "Nom":
                        lstMarque = HibernateMarqueService.Retrieve(txtRecherche.Text);
                        foreach (Marque m in lstMarque)
                        {
                            lstBouteille.AddRange(HibernateBouteilleService.RetrieveByMarqueId((int)m.IdMarque,chbAfficherSupprimee.IsChecked));
                        }
                        break;
                    case "Emplacement":
                        lstEmplacement = HibernateEmplacementService.Retrieve(txtRecherche.Text);
                        foreach (Emplacement m in lstEmplacement)
                        {
                            lstBouteille.AddRange(HibernateBouteilleService.RetrieveByEmplacementId((int)m.IdEmplacement,chbAfficherSupprimee.IsChecked));
                        }
                        break;
                    case "État":
                        lstBouteille.AddRange(HibernateBouteilleService.RetrieveByEtat(txtRecherche.Text,chbAfficherSupprimee.IsChecked));
                        break;
                    case "Type d'alcool":
                        lstType = HibernateTypeAlcoolService.Retrieve(txtRecherche.Text);
                        foreach (TypeAlcool t in lstType)
                        {
                            lstMarque.AddRange(HibernateMarqueService.RetrieveByType(t));
                               
                        }
                        foreach (Marque m in lstMarque)
                        {
                            lstBouteille.AddRange(HibernateBouteilleService.RetrieveByMarqueId((int)m.IdMarque,chbAfficherSupprimee.IsChecked));
                        }
                        break;
                }
               

                if (lstBouteille.Count != 0)
                    dtgInventaire.ItemsSource = lstBouteille;
                else
                {
                    dtgInventaire.ItemsSource = null;
                }
            }
            else
            {
                lstBouteille = HibernateBouteilleService.RetrieveAll(chbAfficherSupprimee.IsChecked);
                /*foreach (Bouteille b in lstBouteille)
                {
                    b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                    b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                    b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
                }*/

                dtgInventaire.ItemsSource = lstBouteille;
            }
        }


        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {
            lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille(chbAfficherSupprimee.IsChecked));

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Pdf Files|*.pdf";
            saveFileDialog1.FileName = "InventaireBouteilles";
            if (saveFileDialog1.ShowDialog() == true)
            {
                //Crée le fichier

                Document doc = new Document();
                FileStream fs = new System.IO.FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfWriter writer = PdfWriter.GetInstance(doc,fs);
                doc.Open();

                //Entête
                iTextSharp.text.Paragraph titre = new iTextSharp.text.Paragraph("Inventaire");
                titre.Alignment = Element.ALIGN_CENTER;
                titre.Font.SetStyle(Font.BOLD);
                titre.Font.Size = 20;
                doc.Add(titre);
                titre = new iTextSharp.text.Paragraph(" ");
                doc.Add(titre);

                //Création du tableau
                PdfPTable table = new PdfPTable(6); //Le paramètre indique le nombre de colonne. S'il manque de cellules pour la dernière rangée, il ne mettra simplement pas la rangée
                table = CreationDesTables.CreerTableInventaire(table, lstBouteilles);
                doc.Add(table);




                string fullPath = System.IO.Path.GetFullPath(saveFileDialog1.FileName);
                doc.Close();
                Process.Start(fullPath);
            

            }
        }

        
        

        private void btnSuppression_Click(object sender, RoutedEventArgs e)
        {
            if (EcranAccueil.employe.IdRole == null)
            {
                FenetreAuthentification FN = new FenetreAuthentification();
                FN.ShowDialog();
            }
            if (EcranAccueil.employe.IdRole != 1 && EcranAccueil.employe.IdRole != null)
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
            }
            if (EcranAccueil.employe.IdRole == 1)
            {                
                if (AuMoinsUneBouteilleSelectionne())
                {
                    MessageBoxResult resultat = MessageBox.Show("Êtes vous sûr de vouloire supprimer la sélection de bouteilles?", "Question", MessageBoxButton.YesNo);

                    if (resultat == MessageBoxResult.Yes)
                    {
                        List<Bouteille> lstBouteille = dtgInventaire.SelectedItems.Cast<Bouteille>().ToList();

                        foreach (var bouteille in lstBouteille)
                        {
                            bouteille.Etat = "Supprimée";
                            bouteille.VolumeRestant = 0;
                            bouteille.IdEmplacement = HibernateEmplacementService.retrieveEmplacementByNom("Aucun")[0].IdEmplacement;
                            HibernateBouteilleService.Update(bouteille);

                        }
                        dtgInventaire.ItemsSource = new ObservableCollection<Bouteille>(ChargerListBouteille(chbAfficherSupprimee.IsChecked));
                    }
                }
            }
        }

        private PdfPTable CreerEnteteFichier(PdfPTable table)
        {


            return table;
        }
        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            ContenuHeader = ((DataGridColumnHeader)sender).Content.ToString();
        }

        private void chbAfficherSupprimee_Checked(object sender, RoutedEventArgs e)
        {
            lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille(true));
            dtgInventaire.ItemsSource = lstBouteilles;


        }

        private void chbAfficherSupprimee_Unchecked(object sender, RoutedEventArgs e)
        {
            lstBouteilles = new ObservableCollection<Bouteille>(ChargerListBouteille(false));
            dtgInventaire.ItemsSource = lstBouteilles;
        }

        private void lblMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
    }
}
