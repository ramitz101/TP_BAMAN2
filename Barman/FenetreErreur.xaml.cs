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
    /// Logique d'interaction pour FenetreErreur.xaml
    /// </summary>
    public partial class FenetreErreur : Window
    {
        public FenetreErreur()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            FenetreAuthentification FN = new FenetreAuthentification();
            FN.ShowDialog();
            this.Close();
        }
    }
}
