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
using Barman.BouteilleDossier;
using Barman.BouteilleDossier.Hibernate;
using Barman.MarqueDossier.Hibernate;
using Barman.TypeDossier.Hibernate;

namespace Barman.CommandeDossier.view
{
    /// <summary>
    /// Logique d'interaction pour EcranRecevoirCommande.xaml
    /// </summary>
    public partial class EcranRecevoirCommande : UserControl
    {
        //private ObservableCollection<Bouteille> listBouteilleCommand;

        private ObservableCollection<List<Bouteille>> listBouteilleCommand;

        public List<List<Bouteille>> ListBouteille { get; set; }
        private Commande commande { get; set; }
        public EcranRecevoirCommande(Commande c)
        {
            InitializeComponent();
            StringBuilder s = new StringBuilder();
            s.Append("Commande du ").Append(c.DateCommande.ToLongDateString());
            lblCommande.Content = s.ToString();
            dtgCommande.CanUserAddRows = false;
            commande = c;

          

           listBouteilleCommand = new ObservableCollection<List<Bouteille>>(ChargerBouteilleCommande((int)c.IdCommande));
           dtgCommande.ItemsSource = listBouteilleCommand;

           
            if(c.Etat == "Reçu" || listBouteilleCommand.ElementAt(0).Count == 0)
            {
                btnConfirmer.IsEnabled = false;
                btnSupprimer.IsEnabled = false;
            }
            

            
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {

            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
            EcranCommande EC = new EcranCommande();
            ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EC);
        }

        private void btnConfirmer_Click(object sender, RoutedEventArgs e)
        {
            if (listBouteilleCommand.Count > 0)
            {
                var result = MessageBox.Show("Êtes-vous sur de vouloir ajouter cette commande à l'inventaire", "Avertissement", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var i in listBouteilleCommand)
                    {
                        foreach (var j in i)
                        {
                            j.IdEmplacement = 8;
                            HibernateBouteilleService.Update(j);
                        }
                    }
                    commande.Etat = "Reçu";

                    MessageBox.Show("La commande à bien été ajouté", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranCommande EC = new EcranCommande();
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EC);
                }
            }
        }

        static public List<List<Bouteille>> ChargerBouteilleCommande(int IdCommande)
        {
            List<Bouteille> listB = new List<Bouteille>();         
            List<List<Bouteille>> listBouteille = new List<List<Bouteille>>();
            listB = HibernateBouteilleService.RetrieveByIdCommande(IdCommande);

            List<Bouteille> listTempo = new List<Bouteille>();
            bool premierCoup = false;
            foreach (var i in listB)
            {
                i.SaMarque = HibernateMarqueService.Retrieve((int)i.IdMarque)[0];
                i.SaMarque.SonTypeAlcool = HibernateTypeAlcoolService.RetrieveTypeAlcool((int)i.SaMarque.IdTypeAlcool)[0];
                
                if (!premierCoup)
                {
                    listTempo.Insert(0, i);
                    premierCoup = true;
                }
                else
                {
                    if (listTempo.ElementAt(0).IdMarque == i.IdMarque)
                    {
                        listTempo.Insert(0, i);
                    }
                    else
                    {
                        listBouteille.Add(listTempo);
                        listTempo = new List<Bouteille>();
                        listTempo.Insert(0, i);
                        
                    }
                }

               
            }

            listBouteille.Add(listTempo);



            return listBouteille;   
        }


        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if(dtgCommande.SelectedItems.Count == 1)
            {
                
                List<Bouteille> listTempo = new List<Bouteille>((List<Bouteille>)dtgCommande.SelectedItem);
                var result = MessageBox.Show(("Êtes-vous sur de vouloir supprimer " + listTempo.ElementAt(0).SaMarque.Nom.ToString()), "Avertissement", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    HibernateBouteilleService.Delete((Bouteille)listTempo.ElementAt(0));

                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.RemoveAt(0);
                    EcranRecevoirCommande EcranRecevoirCommande = new EcranRecevoirCommande(commande);
                    ((MainWindow)System.Windows.Application.Current.MainWindow).GrdPrincipale.Children.Insert(0, EcranRecevoirCommande);
                }
            }
            else
            {
                MessageBox.Show("Vous devez sélectionner une bouteille pour supprimer", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

     
    }
}
