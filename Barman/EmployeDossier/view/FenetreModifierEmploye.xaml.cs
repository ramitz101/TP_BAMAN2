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
using Barman.EmployeDossier.Hibernate;
using Barman.RoleDossier;
using Barman.RoleDossier.Hibernate;

namespace Barman.EmployeDossier.view
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
                txtCode.Text = employe.CodeEmploye;



            }


        }
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationChamps())
            {
                EmployeModifier.Nom = txtNom.Text;
                EmployeModifier.Prenom = txtPrenom.Text;
                EmployeModifier.Telephone = txtTelephone.Text;
                EmployeModifier.NAS = txtNAS.Text;
                EmployeModifier.IdRole = RoleChoisi();
                EmployeModifier.CodeEmploye = txtCode.Text;

                HibernateEmployeService.Update(EmployeModifier);

                this.Close();

            }else
                MessageBox.Show("Une erreur est survenu lors de la modification d'un employe. Vérifier le contenu des champs et réessayé.");
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

        private bool ValidationChamps()
        {
            if (txtNom.Text != "" && txtPrenom.Text != "" && UnRdbEstChoisi() && Employe.ValiderNAS(txtNAS.Text) && Employe.ValiderNumeroTelephone(txtTelephone.Text))
                return true;
            else
                return false;
        }

        public bool UnRdbEstChoisi()
        {
            if (rdbAdministrateur.IsChecked == true || rdbUtilisateur.IsChecked == true)
                return true;
            else
                return false;
        }

        private void btnGenererNouveauCode_Click(object sender, RoutedEventArgs e)
        {
            txtCode.Text = Generer.genererCode(Employe.GetCodeEmployeDejaExistant());
            //txtCode.Text = Employe.genererCodeEmploye();
        }

        private void CalendarModifierEmploye_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
