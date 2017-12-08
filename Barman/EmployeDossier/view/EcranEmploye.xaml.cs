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
    public partial class EcranEmploye : UserControl
    {
        public string ContenuHeader { get; set; }
       



        DependencyObject mainDep = new DependencyObject();
        private static ObservableCollection<Employe> lstEmployes = new ObservableCollection<Employe>();

        public EcranEmploye()
        {
            InitializeComponent();
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye(null));
            dtgEmploye.ItemsSource = lstEmployes;
            dtgEmploye.IsReadOnly = true;
            ContenuHeader = "Nom";

            if (EcranAccueil.Employe.SonRole.Code == Constante.ADMINISTRATEUR)
                App.Current.MainWindow.Title = "Barman - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Administrateur" + " - Employés";
            else
                App.Current.MainWindow.Title = "Barman - " + EcranAccueil.Employe.Prenom + " " + EcranAccueil.Employe.Nom + " - " + "Utilisateur" + " - Employés";
        }

        private void btnGerer_Click(object sender, RoutedEventArgs e)
        {
            if (UnEmployeSelectionne())
            {
                FenetreModifierEmploye popup = new FenetreModifierEmploye(lstEmployes, dtgEmploye.SelectedItem as Employe, this);
                popup.ShowDialog();
            }

            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye(null));
        }

        private void btnAjouterEmploye_Click(object sender, RoutedEventArgs e)
        {
            FenetreAjouterEmploye popup = new FenetreAjouterEmploye();
            popup.ShowDialog();
            dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye(null));
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        private static List<Employe> ChargerListEmploye(bool? inactif)
        {
            List<Employe> listE = new List<Employe>(HibernateEmployeService.RetrieveAll(inactif));
            foreach (var e in listE)
            {
                e.SonRole = HibernateRoleService.Retrieve((int)e.SonRole.IdRole)[0];
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
                MessageBox.Show("Vous devez selectionner un seul employé.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                return false;
            }
            else
            {
                MessageBox.Show("Vous devez selectionner un seul employé.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                return false;
            }
        }

        private void btnSuppression_Click(object sender, RoutedEventArgs e)
        {
            if (AuMoinsUnEmployeSelectionne())
            {
                MessageBoxResult resultat = MessageBox.Show("Êtes-vous sûr de vouloir supprimer les employés sélectionnés?", "Suppression d'employé", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (resultat == MessageBoxResult.Yes)
                {
                    List<Employe> lstEmploye = dtgEmploye.SelectedItems.Cast<Employe>().ToList();
                    foreach (var employe in lstEmploye)
                    {
                        employe.Etat = "I";
                        HibernateEmployeService.Update(employe);
                    }
                    dtgEmploye.ItemsSource = new ObservableCollection<Employe>(ChargerListEmploye(null));
                }
            }
        }

        private bool AuMoinsUnEmployeSelectionne()
        {
            if (dtgEmploye.SelectedItems.Count > 0)
                return true;
            else
            {
                MessageBox.Show("Vous devez selectionner au moins un employé.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
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
                        SiLaListeEstVide(HibernateEmployeService.RetrievePrenom(txtRecherche.Text));
                        break;
                    case "Téléphone":
                        SiLaListeEstVide(HibernateEmployeService.RetrieveTelephone(txtRecherche.Text));
                        break;
                    case "Date d'embauche":
                        SiLaListeEstVide(HibernateEmployeService.RetrieveDateEmbauche(txtRecherche.Text));
                        break;
                    case "NAS":
                        SiLaListeEstVide(HibernateEmployeService.RetrieveNAS(txtRecherche.Text));
                        break;
                    case "Role":
                        lstRole = HibernateRoleService.RetrieveRole(txtRecherche.Text);
                        foreach (Role r in lstRole)
                            lstEmploye.AddRange(HibernateEmployeService.RetrieveRole((int)r.IdRole));
                        SiLaListeEstVide(lstRole);
                        break;
                    case "Code":
                        SiLaListeEstVide(HibernateEmployeService.RetrieveCode(txtRecherche.Text));
                        break;
                    default:
                        SiLaListeEstVide(HibernateEmployeService.RetrieveNom(txtRecherche.Text));
                        break;
                }
            }
            else
                dtgEmploye.ItemsSource = HibernateEmployeService.RetrieveAll(null);
        }

        private void SiLaListeEstVide<T>(List<T> lst)
        {
            if (lst.Count != 0)
                dtgEmploye.ItemsSource = lst;
            else
                dtgEmploye.ItemsSource = null;
        }

        private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
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

            FenetreOptionsImprimer optionsPDF = new FenetreOptionsImprimer();
            optionsPDF.ShowDialog();

            if (Constante.ouvrirEtSauvegarder || Constante.seulementOuvrir || Constante.seulementSauvegarder)
            {
                lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye(null));
                SaveFileDialog saveFileDialog = new SaveFileDialog();               


                saveFileDialog.Filter = "Pdf Files|*.pdf";
                saveFileDialog.FileName = "Employes";
                if (Constante.ouvrirEtSauvegarder || Constante.seulementSauvegarder)
                {

                    if (saveFileDialog.ShowDialog() == true)
                    {


                        //Crée le fichier
                        Document doc = new Document();
                        FileStream fs = new System.IO.FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
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


                        string fullPath = System.IO.Path.GetFullPath(saveFileDialog.FileName);
                        doc.Close();
                        if (Constante.ouvrirEtSauvegarder)
                            Process.Start(fullPath);
                    }
                }
                else
                {                    

                    Directory.CreateDirectory("BarApp");                    

                    //Crée le fichier
                    Document doc = new Document();
                    FileStream fs = new System.IO.FileStream("BarApp\\Employes.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
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


                    string fullPath = System.IO.Path.GetFullPath("BarApp\\Employes.pdf");
                    doc.Close();

                    Process.Start(fullPath);

                }
            }
        }

        private void chbAfficherEmployeInactif_Checked(object sender, RoutedEventArgs e)
        {
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye(true));
            dtgEmploye.ItemsSource = lstEmployes;
        }

        private void chbAfficherEmployeInactif_Unchecked(object sender, RoutedEventArgs e)
        {
            lstEmployes = new ObservableCollection<Employe>(ChargerListEmploye(false));
            dtgEmploye.ItemsSource = lstEmployes;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnGerer_Click(sender, e);
        }
    }
}
