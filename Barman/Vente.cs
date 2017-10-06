using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Vente
    {
        public virtual int? IdVente { get; set; }
        public virtual int? IdEmploye { get; set; }
        public virtual int? IdBouteille { get; set; }

        public virtual float? PrixOnce { get; set; }

        public virtual DateTime DateVente { get; set; }

        public virtual int? Volume { get; set; }

        public virtual Employe UnEmploye { get; set; }


        public Vente()
        {
            IdVente = null;
            IdEmploye = null;
            IdBouteille = null;
            PrixOnce = null;
            DateVente = DateTime.MinValue;
            Volume = null;

            UnEmploye = new Employe();
        }
    }
}
