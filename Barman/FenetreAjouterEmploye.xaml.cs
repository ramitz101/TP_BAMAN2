using System;
using System.Collections.Generic;
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

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour FenetreAjouterEmploye.xaml
    /// </summary>
    public partial class FenetreAjouterEmploye : Window
    {
        public FenetreAjouterEmploye()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            calendarDate.SelectedDate = DateTime.Now;
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {           


            HibernateEmployeService.Create(new Employe(txtNom.Text, txtPrenom.Text, txtTelephone.Text, txtNAS.Text, calendarDate.SelectedDate.Value, RoleChoisi()));
            this.Close();
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
    }
}
