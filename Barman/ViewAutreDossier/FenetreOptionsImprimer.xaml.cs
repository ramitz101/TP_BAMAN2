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
using Barman.EmployeDossier.view;

namespace Barman.ViewAutreDossier
{
    /// <summary>
    /// Logique d'interaction pour OptionsImprimer.xaml
    /// </summary>
    public partial class FenetreOptionsImprimer : Window
    {
        public EcranEmploye ecranEmploye { get; set; }

        public FenetreOptionsImprimer(EcranEmploye ecranEmpl)
        {
            InitializeComponent();
            ecranEmploye = ecranEmpl;
            this.Owner = App.Current.MainWindow;
        }

        private void btnSeulementOuvrir_Click(object sender, RoutedEventArgs e)
        {
            ecranEmploye.seulementOuvrir = true;
            this.Close();
        }

        private void btnSeulementSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            ecranEmploye.seulementSauvegarder = true;
            this.Close();
        }

        private void btnSauvegarderEtOuvrir_Click(object sender, RoutedEventArgs e)
        {
            ecranEmploye.ouvrirEtSauvegarder = true;
            this.Close();
        }
    }
}
