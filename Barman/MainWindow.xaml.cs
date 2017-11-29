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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Barman.ViewAutreDossier;
using System.IO;

namespace Barman
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EcranAccueil EcranAccueil { get; set; }
        public bool NonXClose { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            EcranAccueil = new EcranAccueil();
            GrdPrincipale.Children.Add(EcranAccueil);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }
        private void Window_Closed(object sender,EventArgs e)
        {
            string path = System.IO.Path.GetTempPath();
            //path = path + "Temp\\";
            //Directory.Delete(path, true);
        }        

    }
}
