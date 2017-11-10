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
using System.Text.RegularExpressions;


namespace Barman.EmployeDossier.view
{
    /// <summary>
    /// Logique d'interaction pour FenetreAjouterEmploye.xaml
    /// </summary>
    public partial class FenetreAjouterEmploye : Window
    {
        private String oldValueName = String.Empty;
        private String oldValuePrenom = String.Empty;
        private String oldValueTelephone = String.Empty;
        private String oldValueNAS = String.Empty;
        

        private bool btnConfirmerHasBeemClicked = false;

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
                HibernateEmployeService.Create(new Employe(txtNom.Text, txtPrenom.Text, Extractdigits(txtTelephone.Text), Extractdigits(txtNAS.Text), calendarDate.SelectedDate.Value, RoleChoisi()));
                this.Close();

            }

            //btnConfirmerHasBeemClicked = false;
        }





        private string Extractdigits(string pText)
        {
            string text = pText;
            string textResult = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsDigit(text[i]))
                    textResult += text[i];
            }
            return textResult;
        }

        private bool ValidationChamps()
        {            
            bool estValide = true;
            
            if (txtNom.Text != "" && txtNom.Text.Length < 100 && txtPrenom.Text != "" && txtPrenom.Text.Length < 100  && UnRdbEstChoisi() && txtNAS.Text.Count(Char.IsDigit) == 9 && txtTelephone.Text.Count(Char.IsDigit) == 10)
            {
                estValide = true;
            }
            if (txtNom.Text == "")
            {                                
                txtNom.ToolTip = "Vous devez entrer un nom.";
                txtNom.BorderBrush = System.Windows.Media.Brushes.Red;
                estValide = false;
            }
            if (txtNom.Text.Length > 100)
            {                
                txtNom.ToolTip = "Le nom entré est trop long.";
                txtNom.BorderBrush = System.Windows.Media.Brushes.Red;
                estValide = false;
            }
            
            if (txtPrenom.Text == "")
            {                
                txtPrenom.ToolTip = "Vous devez entrer un prénom.";
                txtPrenom.BorderBrush = System.Windows.Media.Brushes.Red;
                estValide = false;
            }
            if (txtPrenom.Text.Length > 100)
            {               
                txtPrenom.ToolTip = "Le prénom entré est trop long.";
                txtPrenom.BorderBrush = System.Windows.Media.Brushes.Red;
                estValide = false;
            }          
            if (!UnRdbEstChoisi())
            {          
                lblRole.ToolTip = "Vous devez sélectionner un niveau d'accès.";
                lblRole.Foreground = Brushes.Red;
                estValide = false;
            }
            if (txtNAS.Text.Count(Char.IsDigit) == 0)
            {               
                txtNAS.ToolTip = "Vous devez entrer un numéro d'assurance social.";
                txtNAS.BorderBrush = Brushes.Red;
                estValide = false;
            }
            if (txtNAS.Text.Count(Char.IsDigit) < 9 && txtNAS.Text.Count(Char.IsDigit) > 0)
            {        
                txtNAS.ToolTip = "Le numéro d'assurance social n'est pas complet.";
                txtNAS.BorderBrush = new SolidColorBrush(Colors.Red);
                estValide = false;
            }
            if (txtTelephone.Text.Count(Char.IsDigit) == 0)
            {               
                txtTelephone.ToolTip = "Vous devez entrer un numéro de téléphone.";
                txtTelephone.BorderBrush = new SolidColorBrush(Colors.Red);
                estValide = false;
            }
            if (txtTelephone.Text.Count(Char.IsDigit) < 10 && txtTelephone.Text.Count(Char.IsDigit) > 0)
            {              
                txtTelephone.ToolTip = "Le numéro de téléphone n'est pas complet.";
                txtTelephone.BorderBrush = new SolidColorBrush(Colors.Red);
                estValide = false;
            }
            
            lblInfoMessage.Foreground = Brushes.Red;
            lblInfoMessage.Content = "Une erreur est survenue. Passé votre souris sur le rouge pour en connaitre la cause.";
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

        private void txtNom_GotFocus(object sender, RoutedEventArgs e)
        {          
                oldValueName = txtNom.Text;            
        }

        private void txtNom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(oldValueName == txtNom.Text))
            {
                txtNom.ClearValue(Border.BorderBrushProperty);
                txtNom.ToolTip = "Le nom du nouvel employé";
            }
        }

        private void txtPrenom_GotFocus(object sender, RoutedEventArgs e)
        {
            oldValuePrenom = txtPrenom.Text;
        }

        private void txtPrenom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(oldValuePrenom == txtPrenom.Text))
            {
                txtPrenom.ClearValue(Border.BorderBrushProperty);
                txtPrenom.ToolTip = "Le prénom du nouvel employé";
            }
        }

        private void txtTelephone_GotFocus(object sender, RoutedEventArgs e)
        {
            oldValueTelephone = txtTelephone.Text;
        }

        private void txtTelephone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(oldValueTelephone == txtTelephone.Text))
            {
                txtTelephone.ClearValue(Border.BorderBrushProperty);
                txtTelephone.ToolTip = "Le numéro de téléphone pour rejoindre l'employé";
            }
        }

        private void txtNAS_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(oldValueNAS == txtNAS.Text))
            {
                txtNAS.ClearValue(Border.BorderBrushProperty);
                txtNAS.ToolTip = "Le numéro d'assurance social";
            }
        }

        private void txtNAS_GotFocus(object sender, RoutedEventArgs e)
        {
            oldValueNAS = txtNAS.Text;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
