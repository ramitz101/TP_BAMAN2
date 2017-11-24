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
using Barman.BouteilleDossier.Hibernate;
using Barman.CommandeDossier.Hibernate;
using Barman.EmployeDossier.Hibernate;
using Barman.ViewAutreDossier;
using Barman.BouteilleDossier;
using Microsoft.Win32;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace Barman.CommandeDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranCommande.xaml
    /// </summary>
    public partial class EcranCommande : UserControl
    {


        private static ObservableCollection<Commande> lstCommandes;

        private static List<Commande> ChargerListCommande()
        {
            List<Commande> listC = new List<Commande>(HibernateCommandeService.RetrieveAll());
            foreach(var i in listC)
            {
                i.ListBouteille = HibernateBouteilleService.RetrieveByIdCommande((int)i.IdCommande);
                i.UnEmploye = HibernateEmployeService.Retrieve((int)i.IdEmploye)[0];
            }
            return listC;
        }

        public EcranCommande()
        {
            InitializeComponent();
            lstCommandes = new ObservableCollection<Commande>(ChargerListCommande());
            dtgCommande.CanUserAddRows = false;
            
            dtgCommande.ItemsSource = lstCommandes;
            if (EcranAccueil.employe.SonRole.Code == "Utils")
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom + " - " + "Utilisateur" + " - Commandes";
            else
                App.Current.MainWindow.Title = "Barmans - " + EcranAccueil.employe.Prenom + " " + EcranAccueil.employe.Nom + " - " + "Administrateur" + " - Commandes";

        }

        private void btnNouvelleCommande_Click(object sender, RoutedEventArgs e)
        {
            if (EcranAccueil.employe.IdRole == null)
            {
                FenetreAuthentification FN = new FenetreAuthentification();
                FN.ShowDialog();
            }
            if (EcranAccueil.employe.SonRole.Code == "Utils" && EcranAccueil.employe.IdRole != null)
            {
                FenetreErreur FE = new FenetreErreur();
                FE.ShowDialog();
            }
            if (EcranAccueil.employe.SonRole.Code == "Admin")
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranNouvelleCommande EcranNouvelCommand = new EcranNouvelleCommande();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EcranNouvelCommand);
            }
        }

        private void btnAccueil_Click(object sender, RoutedEventArgs e)
        {
            EcranAccueil EA = new EcranAccueil();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EA);
        }

        private void btnRecevoirCommande_Click(object sender, RoutedEventArgs e)
        {
            if (dtgCommande.SelectedItems.Count == 1)
            {
                if (EcranAccueil.employe.IdRole == null)
                {
                    FenetreAuthentification FN = new FenetreAuthentification();
                    FN.ShowDialog();
                }
                if (EcranAccueil.employe.SonRole.Code == "Utils" && EcranAccueil.employe.IdRole != null)
                {
                    FenetreErreur FE = new FenetreErreur();
                    FE.ShowDialog();
                    
                }
                if (EcranAccueil.employe.SonRole.Code == "Admin")
                {
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranRecevoirCommande EcranRecevoirCommande = new EcranRecevoirCommande((Commande)dtgCommande.SelectedItem);
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EcranRecevoirCommande);
                }
            }
            else
            {
                MessageBox.Show("Vous devez selectionner une commande","Mauvaise sélection",MessageBoxButton.OK,MessageBoxImage.Warning,MessageBoxResult.OK);
            }
        }

        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {
            

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Pdf Files|*.pdf";
            saveFileDialog1.FileName = "Commandes";

            if (saveFileDialog1.ShowDialog() == true)
            {
                //Crée le fichier

                Document doc = new Document();
                FileStream fs = new System.IO.FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //Entête
                iTextSharp.text.Paragraph titre = new iTextSharp.text.Paragraph("Commandes");
                titre.Alignment = Element.ALIGN_CENTER;
                titre.Font.SetStyle(Font.BOLD);
                titre.Font.Size = 20;
                doc.Add(titre);
                titre = new iTextSharp.text.Paragraph(" ");
                doc.Add(titre);

                //Création du tableau
                PdfPTable table = new PdfPTable(4); //Le paramètre indique le nombre de colonne. S'il manque de cellules pour la dernière rangée, il ne mettra simplement pas la rangée
                table = CreationDesTables.CreerTableCommande(table, lstCommandes);
                doc.Add(table);




                string fullPath = System.IO.Path.GetFullPath(saveFileDialog1.FileName);
                doc.Close();
                Process.Start(fullPath);


            }
        }
    }
}
