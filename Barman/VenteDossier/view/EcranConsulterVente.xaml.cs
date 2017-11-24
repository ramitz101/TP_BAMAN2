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
    /// Logique d'interaction pour EcranConsulterVente.xaml
    /// </summary>
    public partial class EcranConsulterVente : UserControl
    {
        private ObservableCollection<Vente> lstVente;
        private List<DateTime> lstworkDate;


        public EcranConsulterVente()
        {
            InitializeComponent();
            
            cldVente.SelectedDate = DateTime.Now;

            List<Vente> lstAllVente = new List<Vente>();
            lstAllVente = HibernateVenteService.RetrieveAllVenteEmploye((int)EcranAccueil.Employe.IdEmploye);
            lstworkDate = new List<DateTime>();
            foreach(var i in lstAllVente)
            {
                lstworkDate.Add(i.DateVente);

            }


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
                        catch(Exception ex) { }
                    }
                }


            }
            
           
            
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Clear();
            EcranOnglets EO = new EcranOnglets(2);
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Add(EO);
        }

       

        private void cldVente_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstworkDate != null)
            {
                foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(cldVente.SelectedDate.Value.Year, cldVente.SelectedDate.Value.Month)))
                {
                    bool trouve = false;
                    foreach (var i in lstworkDate)
                    {
                        if (i.Day == day)
                            trouve = true;
                    }

                    if (!trouve)
                    {
                        DateTime b = new DateTime(cldVente.SelectedDate.Value.Year, cldVente.SelectedDate.Value.Month, day);
                        try
                        {
                            cldVente.BlackoutDates.Add(new CalendarDateRange(b));
                        }
                        catch (Exception ex) { }
                    }
                }

            }



            DateTime? d = cldVente.SelectedDate;
            List<Vente> LalistPourCollection = new List<Vente>();

            LalistPourCollection = HibernateVenteService.RetrieveVenteEmploye((int)EcranAccueil.Employe.IdEmploye, (DateTime)d);
            lstVente = new ObservableCollection<Vente>(LalistPourCollection);
            foreach(var i in lstVente)
            {
                i.laBouteille = HibernateBouteilleService.Retrieve((int)i.IdBouteille)[0];
                i.laBouteille.SaMarque = HibernateMarqueService.Retrieve((int)i.laBouteille.IdMarque)[0];
            }
            dtgVenteEmploye.ItemsSource = lstVente;
            Mouse.Capture(null);

        }

        private void btnImprimer_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Pdf Files|*.pdf";

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

       

        private void chbToutVente_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chbToutVente.IsChecked)
            {
                List<Vente> lstAllVente = new List<Vente>();
                lstAllVente = HibernateVenteService.RetrieveAllVenteEmploye((int)EcranAccueil.Employe.IdEmploye);
                foreach (var i in lstAllVente)
                {
                    i.laBouteille = HibernateBouteilleService.Retrieve((int)i.IdBouteille)[0];
                    i.laBouteille.SaMarque = HibernateMarqueService.Retrieve((int)i.laBouteille.IdMarque)[0];
                }
                dtgVenteEmploye.ItemsSource = lstAllVente;
            }
            else
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                EcranConsulterVente ec = new EcranConsulterVente();
                ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, ec);
            }
        }
    }
}
