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
using System.Windows.Shapes;

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour FenetreModifierEmploye.xaml
    /// </summary>
    public partial class FenetreModifierEmploye : Window
    {

        private Employe EmployeModifier;
        public FenetreModifierEmploye(ObservableCollection<Employe> lstEmploye, Employe employe, EcranEmploye ecranEmploye)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            if (employe != null)
            {


                EmployeModifier = employe;
                txtNom.Text = employe.Nom;
                txtPrenom.Text = employe.Prenom;
                txtTelephone.Text = employe.Telephone;
                txtNAS.Text = employe.NAS;
                CalendarModifierEmploye.SelectedDate = employe.DateEmbauche;
                CalendarModifierEmploye.DisplayDate = employe.DateEmbauche;
                // à améliorer
                if (employe.IdRole == 1)
                    rdbAdministrateur.IsChecked = true;
                else
                    rdbUtilisateur.IsChecked = true;



            }


        }
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            EmployeModifier.Nom = txtNom.Text;
            EmployeModifier.Prenom = txtPrenom.Text;
            EmployeModifier.Telephone = txtTelephone.Text;
            EmployeModifier.NAS = txtNAS.Text;
            EmployeModifier.IdRole = RoleChoisi();

            HibernateEmployeService.Update(EmployeModifier);

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
