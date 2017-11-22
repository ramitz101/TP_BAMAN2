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
using Barman.EmployeDossier;
using Barman.EmployeDossier.Hibernate;
using Barman.MarqueDossier.Hibernate;
using Barman.VenteDossier.Hibernate;
using Barman.ViewAutreDossier;
using Microsoft.Win32;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Diagnostics;

namespace Barman.VenteDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranGererVente.xaml
    /// </summary>
    public partial class EcranGererVente : UserControl
    {
        private ObservableCollection<Employe> lstEmploye = new ObservableCollection<Employe>(ChargerListEmploye());
        private ObservableCollection<Vente> lstVente;
        private List<Vente> lstAllVente;
        private List<DateTime> lstworkDate;

        public EcranGererVente()
        {
            InitializeComponent();
            dtgVenteEmploye.CanUserAddRows = false;
            cldVente.SelectedDate = DateTime.Now;
            dtgVenteEmploye.SelectedValuePath = "Vente";

            // Ajout d'un employé fixif tous dans le comboBox employé
            Employe t = new Employe();
            t.IdEmploye = -1;
            t.Nom = "Tous";
            lstEmploye.Add(t);
            cboEmploye.ItemsSource = lstEmploye;
            cboEmploye.DisplayMemberPath = "Nom";
            cboEmploye.SelectedValuePath = "IdEmploye";
            cboEmploye.SelectedItem = EcranAccueil.employe; // met l'employé connecté par défaut
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(2);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Pdf Files|*.pdf";
            saveFileDialog1.FileName = "Ventes";
            if (saveFileDialog1.ShowDialog() == true)
            {
                //Crée le fichier

                Document doc = new Document();
                FileStream fs = new System.IO.FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //Entête
                iTextSharp.text.Paragraph titre = new iTextSharp.text.Paragraph("Ventes");
                titre.Alignment = Element.ALIGN_CENTER;
                titre.Font.SetStyle(Font.BOLD);
                titre.Font.Size = 20;
                doc.Add(titre);
                titre = new iTextSharp.text.Paragraph(" ");
                doc.Add(titre);

                //Création du tableau
                PdfPTable table = new PdfPTable(4); //Le paramètre indique le nombre de colonne. S'il manque de cellules pour la dernière rangée, il ne mettra simplement pas la rangée
                table = CreationDesTables.CreerTableVente(table, lstVente);
                doc.Add(table);

                
                string fullPath = System.IO.Path.GetFullPath(saveFileDialog1.FileName);
                doc.Close();
                Process.Start(fullPath);

            }
        }

        private static List<Employe> ChargerListEmploye()
        {
            List<Employe> listE = new List<Employe>(HibernateEmployeService.RetrieveAll(null));
            return listE;
        }
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dtgVenteEmploye.SelectedItems.Count == 1)
            {
                try
                {
                    Vente v = new Vente();
                    v = (Vente)dtgVenteEmploye.SelectedItem;
                    var result = MessageBox.Show("Êtes vous sur de vouloir supprimer ","Avertissement", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        HibernateVenteService.Delete(v);                      
                        RefreshList();
                    }

                    // Remettre la quantité dans la bouteille
                    v.laBouteille.VolumeRestant = v.laBouteille.VolumeRestant + v.Volume;
                    if (v.laBouteille.VolumeInitial == v.laBouteille.VolumeRestant)
                        v.laBouteille.Etat = "Pleine";

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Une erreur est survenu "+ ex, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void cboEmploye_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            cldVente.BlackoutDates.Clear();
            lstAllVente = new List<Vente>();
            if ((int)cboEmploye.SelectedValue == -1)
            {
                lstAllVente = HibernateVenteService.RetrieveAll();
            }
            else
            {
                lstAllVente = HibernateVenteService.RetrieveAllVenteEmploye((int)cboEmploye.SelectedValue);
            }
            lstworkDate = new List<DateTime>();
            foreach (var i in lstAllVente)
            {
                lstworkDate.Add(i.DateVente);

            }
            if (lstworkDate != null)
            {
                for (int i = 1; i < 13; i++)
                {
                    foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(cldVente.SelectedDate.Value.Year, i)))
                    {
                        bool trouve = false;
                        foreach (var j in lstworkDate)
                        {
                            if (j.Day == day && j.Month == i)
                                trouve = true;
                        }

                        if (!trouve)
                        {
                            try
                            {
                                cldVente.BlackoutDates.Add(new CalendarDateRange(new DateTime(cldVente.SelectedDate.Value.Year, i, day)));
                            }
                            catch (Exception ex) { }
                        }
                    }


                }
            }
            RefreshList();
        }

        private void cldVente_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }


        private void RefreshList()
        {
            DateTime? d = cldVente.SelectedDate;
            List<Vente> LalistPourCollection = new List<Vente>();

          
            if (d == null)
                d = DateTime.Today;
            if (cboEmploye.SelectedValue != null)
            {
                if ((int)cboEmploye.SelectedValue != -1)
                {
                    LalistPourCollection = HibernateVenteService.RetrieveVenteEmploye((int)cboEmploye.SelectedValue, (DateTime)d);

                }
                else
                {
                    foreach (var i in lstEmploye)
                    {
                        try
                        {
                            foreach (var k in HibernateVenteService.RetrieveVenteEmploye((int)i.IdEmploye, (DateTime)d))
                            {
                                LalistPourCollection.Add(k);
                            }
                        }
                        catch (Exception ex) { } // Le try catch ici sert pour l'employe (tous) qui a id -1..Empeche de planter.
                    }
                }
                lstVente = new ObservableCollection<Vente>(LalistPourCollection);
                foreach (var i in lstVente)
                {
                    i.laBouteille = HibernateBouteilleService.Retrieve((int)i.IdBouteille)[0];
                    i.laBouteille.SaMarque = HibernateMarqueService.Retrieve((int)i.laBouteille.IdMarque)[0];
                }
                dtgVenteEmploye.ItemsSource = lstVente;
            }
            Mouse.Capture(null);
        }

       
    }
}
