using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Commande
    {
        public virtual int? IdCommande { get; set; }
        public virtual int? IdEmploye { get; set; }

        public virtual DateTime DateCommande { get; set; }

        public virtual Employe UnEmploye { get; set; }

        public virtual IList<Bouteille> ListBouteille { get; set; }

        public Commande()
        {
            IdCommande = null;
            IdEmploye = null;
            DateCommande = DateTime.MinValue;
            UnEmploye = new Employe();
            ListBouteille = new List<Bouteille>();
        }
    }
}
