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
        

        public FenetreOptionsImprimer()
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
           Constante.seulementOuvrir = false;
           Constante.seulementSauvegarder = false;
           Constante.ouvrirEtSauvegarder = false;
        }

        private void btnSeulementOuvrir_Click(object sender, RoutedEventArgs e)
        {
            Constante.seulementOuvrir = true;
            this.Close();
        }

        private void btnSeulementSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            Constante.seulementSauvegarder = true;
            this.Close();
        }

        private void btnSauvegarderEtOuvrir_Click(object sender, RoutedEventArgs e)
        {
            Constante.ouvrirEtSauvegarder = true;
            this.Close();
        }
    }
}
