using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Barman.EmployeDossier.Hibernate;
using Barman.RoleDossier.Hibernate;
using Barman.ViewAutreDossier;
using Barman.RoleDossier;

namespace Barman.EmployeDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranEmploye.xaml
    /// </summary>
    /// 

    // dfasdfasdf
    public partial class EcranEmploye : UserControl
    {
        public string ContenuHeader { get; set; }

        private static ObservableCollection<Employe> lstEmployes = new ObservableCollection<Employe>();

        public EcranEmploye()
        {
            InitializeComponent();
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye());
            dtgEmploye.ItemsSource = lstEmployes;
            dtgEmploye.IsReadOnly = true;
            ContenuHeader = "Nom";

        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (UnEmployeSelectionne())
            {
                FenetreModifierEmploye popup = new FenetreModifierEmploye(lstEmployes, dtgEmploye.SelectedItem as Employe, this);
                popup.ShowDialog();

            }

            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
            //dtgEmploye.Items.Refresh();
        }

        private void btnAjouterEmploye_Click(object sender, RoutedEventArgs e)
        {
            FenetreAjouterEmploye popup = new FenetreAjouterEmploye();
            popup.ShowDialog();

            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
            //dtgEmploye.Items.Refresh();
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
            foreach (var i in listE)
            {
                i.SonRole = HibernateRoleService.Retrieve((int)i.IdRole)[0];
            }
            return listE;
        }

        private bool UnEmployeSelectionne()
        {
            if (dtgEmploye.SelectedItems.Count == 1)
                return true;
            // si le user a sélectionné plus d'une inscription 
            else if (dtgEmploye.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vous avez trops d'employé selectionnées");
                return false;
            }
            else
            {
                MessageBox.Show("Vous devez selectionner un employé");
                return false;
            }
        }

        private void btnSuppression_Click(object sender, RoutedEventArgs e)
        {
            if (AuMoinsUnEmployeSelectionne())
            {

                MessageBoxResult resultat = MessageBox.Show("Êtes vous sûr de vouloire supprimer la sélection d'employés?", "Question", MessageBoxButton.YesNo);

                if (resultat == MessageBoxResult.Yes)
                {
                    List<Employe> lstEmploye = dtgEmploye.SelectedItems.Cast<Employe>().ToList();

                    foreach (var employe in lstEmploye)
                    {
                        HibernateEmployeService.Delete(employe);
                    }
                    dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye());
                }

            }
        }

        private bool AuMoinsUnEmployeSelectionne()
        {
            if (dtgEmploye.SelectedItems.Count > 0)
                return true;
            else
            {
                MessageBox.Show("Vous devez selectionner un employe");
                return false;
            }
        }

        private void txtRecherche_KeyUp_1(object sender, KeyEventArgs e)
        {
            List<Employe> lstEmploye = new List<Employe>();
            List<Role> lstRole = new List<Role>();
            if (txtRecherche.Text != "" && txtRecherche.Text != "Recherche")
            {
                switch (ContenuHeader)
                {
                    case "Prénom":
                        lstEmploye = HibernateEmployeService.RetrievePrenom(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;

                        break;
                    case "Téléphone":
                        lstEmploye = HibernateEmployeService.RetrieveTelephone(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Date d'embauche":
                        lstEmploye = HibernateEmployeService.RetrieveDateEmbauche(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "NAS":
                        lstEmploye = HibernateEmployeService.RetrieveNAS(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Role":
                        lstRole = HibernateRoleService.RetrieveRole(txtRecherche.Text);
                        foreach (Role r in lstRole)
                        {
                            lstEmploye.AddRange(HibernateEmployeService.RetrieveRole((int)r.IdRole));
                        }

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    case "Code":
                        lstEmploye = HibernateEmployeService.RetrieveCode(txtRecherche.Text);

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                    default:
                        lstEmploye = HibernateEmployeService.RetrieveNom(txtRecherche.Text);
                        /*foreach (Employe employe in lstEmploye)
                        {
                            lstEmploye.AddRange(HibernateEmployeService.RetrieveByMarqueId((int)m.IdMarque));
                        }*/

                        if (lstEmploye.Count != 0)
                            dtgEmploye.ItemsSource = lstEmploye;
                        else
                            dtgEmploye.ItemsSource = null;
                        break;
                }
            }
            else
            {
                lstEmploye = HibernateEmployeService.RetrieveAll();
                dtgEmploye.ItemsSource = lstEmploye;
            }
        }

        private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            //List<Employe> lstEmploye = HibernateEmployeService.RetrieveAll();
            /*foreach (Bouteille b in lstBouteille)
            {
                b.SaMarque = HibernateMarqueService.Retrieve((int)b.IdMarque)[0];
                b.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)b.SaMarque.IdTypeAlcool)[0];
                b.SonEmplacement = HibernateEmplacementService.retrieveEmplacement((int)b.IdEmplacement)[0];
            }*/
            //dtgEmploye.ItemsSource = lstEmploye;
        }

        private void txtRecherche_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "Rechercher";
        }

        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            ContenuHeader = ((DataGridColumnHeader)sender).Content.ToString();
        }

        private void btnImprimerEmploye_Click(object sender, RoutedEventArgs e)
        {
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye());

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Pdf Files|*.pdf";
            saveFileDialog1.FileName = "Employes";
            if (saveFileDialog1.ShowDialog() == true)
            {
                //Crée le fichier

                Document doc = new Document();
                FileStream fs = new System.IO.FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //Entête
                iTextSharp.text.Paragraph titre = new iTextSharp.text.Paragraph("Employé");
                titre.Alignment = Element.ALIGN_CENTER;
                titre.Font.SetStyle(Font.BOLD);
                titre.Font.Size = 20;
                doc.Add(titre);
                titre = new iTextSharp.text.Paragraph(" ");
                doc.Add(titre);

                //Création du tableau
                PdfPTable table = new PdfPTable(7); //Le paramètre indique le nombre de colonne. S'il manque de cellules pour la dernière rangée, il ne mettra simplement pas la rangée
                table = CreationDesTables.CreerTableEmploye(table, lstEmployes);
                doc.Add(table);

                
                string fullPath = System.IO.Path.GetFullPath(saveFileDialog1.FileName);
                doc.Close();
                Process.Start(fullPath);

            }
        }
    }
}
