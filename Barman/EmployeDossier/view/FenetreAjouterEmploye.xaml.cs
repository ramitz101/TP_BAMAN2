using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Barman.EmployeDossier.Hibernate;
using Barman.RoleDossier;
using Barman.RoleDossier.Hibernate;

namespace Barman.EmployeDossier.view
{
   /// <summary>
   /// Logique d'interaction pour FenetreAjouterEmploye.xaml
   /// </summary>
   public partial class FenetreAjouterEmploye : Window
   {
      public const int WM_NCPAINT = 0x85;
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC(IntPtr hWnd);
      [DllImport("user32.dll")]
      public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
      public FenetreAjouterEmploye()
      {
         InitializeComponent();
         this.Owner = App.Current.MainWindow;
         calendarDate.SelectedDate = DateTime.Now;
         rdbUtilisateur.IsChecked = true;
      }

      private void btnAnnuler_Click(object sender, RoutedEventArgs e)
      {
         this.Close();
      }

      private void btnConfirmer_Click(object sender, RoutedEventArgs e)
      {
         if (ValidationChamps())
         {
            HibernateEmployeService.Create(new Employe(txtNom.Text, txtPrenom.Text, txtTelephone.Text, txtNAS.Text, calendarDate.SelectedDate.Value, RoleChoisi()));
            this.Close();

         }
         else
         {
           
         }

      }

      private bool ValidationChamps()
      {
         bool estValide = true;
         StringBuilder sb = new StringBuilder();
         if (txtNom.Text != "" && txtNom.Text.Length < 100 && txtPrenom.Text != "" && txtPrenom.Text.Length < 100 && UnRdbEstChoisi() && txtNAS.Text.Count(Char.IsDigit) == 9 && txtTelephone.Text.Count(Char.IsDigit) == 10)
         {
            estValide = true;
         }
         if (txtNom.Text == "")
         {
            sb.AppendLine("Vous devez entrer un nom.");
            txtNom.BorderBrush = System.Windows.Media.Brushes.Red;
            estValide = false;

         }
         if (txtNom.Text.Length > 100)
         {
            sb.AppendLine("Le nom entré est trop long.");
            txtNom.BorderBrush = System.Windows.Media.Brushes.Red;
            estValide = false;
         }
         if (txtPrenom.Text == "")
         {
            sb.AppendLine("Vous devez entrer un prénom.");
            txtPrenom.BorderBrush = System.Windows.Media.Brushes.Red;
            estValide = false;
         }
         if (txtPrenom.Text.Length > 100)
         {
            sb.AppendLine("Le prénom entré est trop long.");
            txtPrenom.BorderBrush = System.Windows.Media.Brushes.Red;
            estValide = false;
         }
         if (!UnRdbEstChoisi())
         {
            sb.AppendLine("Vous devez sélectionner un niveau d'accès.");
            //txtN.BorderBrush = System.Windows.Media.Brushes.Red;
            estValide = false;
         }
         if (txtNAS.Text.Count(Char.IsDigit) == 0)
         {            
            sb.AppendLine("Vous devez entrer un numéro d'assurance social.");
            txtNAS.BorderBrush = Brushes.Red;
            
            estValide = false;
         }
         if (txtNAS.Text.Count(Char.IsDigit) < 9 && txtNAS.Text.Count(Char.IsDigit) > 0)
         {
            sb.AppendLine("Le numéro d'assurance social n'est pas complet.");
            txtNAS.BorderBrush = new SolidColorBrush(Colors.Red);
            estValide = false;
         }
         if (txtTelephone.Text.Count(Char.IsDigit) == 0)
         {
            sb.AppendLine("Vous devez entrer un numéro de téléphone.");
            txtTelephone.BorderBrush = new SolidColorBrush(Colors.Red);
            estValide = false;
         }
         if (txtTelephone.Text.Count(Char.IsDigit) < 10 && txtTelephone.Text.Count(Char.IsDigit) > 0)
         {
            sb.AppendLine("Le numéro de téléphone n'est pas complet.");
            txtTelephone.BorderBrush = new SolidColorBrush(Colors.Red);
            estValide = false;
         }
         sb.Insert(0,"Les erreurs suivantes se sont produite : ");
         lblInfoMessage.Content = sb;
         lblInfoMessage.Foreground = Brushes.Red;
         return estValide;
      }
      

      public bool UnRdbEstChoisi()
      {
         if (rdbAdministrateur.IsChecked == true || rdbUtilisateur.IsChecked == true)
            return true;
         else
            return false;
      }

      private int RoleChoisi()
      {

         if (rdbAdministrateur.IsChecked == true)
         {
            List<Role> lstRole = new List<Role>(HibernateRoleService.Retrieve("Admin"));
            return (int)lstRole[0].IdRole;
         }
         else
         {
            List<Role> lstRole = new List<Role>(HibernateRoleService.Retrieve("Utils"));
            return (int)lstRole[0].IdRole;
         }



      }

      private void calendarDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
      {
         Mouse.Capture(null);
      }
   }
}
