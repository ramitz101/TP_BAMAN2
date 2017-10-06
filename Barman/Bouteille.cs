using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Bouteille
    {
        public virtual int? IdBouteille { get; set; }
        public virtual string Numero { get; set; }
        public virtual int? VolumeRestant { get; set; }
        public virtual int? VolumeInitial { get; set; }
        public virtual float? PrixBouteille { get; set; }
        public virtual string Etat { get; set; }

        public virtual int? IdEmplacement { get; set; }
        public virtual int? IdMarque { get; set; }
        public virtual int? IdCommande { get; set; }

        public virtual Emplacement SonEmplacement { get; set; }

        public virtual Marque SaMarque { get; set; }
        

        public Bouteille()
        {
            IdBouteille = null;
            Numero = String.Empty;
            VolumeInitial = null;
            VolumeRestant = null;
            Etat = String.Empty;
            PrixBouteille = null;
            IdEmplacement = null;
            IdCommande = null;
            IdMarque = null;
            SonEmplacement = new Emplacement();
            SaMarque = new Marque();

        }

        public Bouteille(Marque saMarque, int volumeInitial, string codeSAQ)
        {
            SaMarque = saMarque;
            VolumeInitial = volumeInitial;
            //codeSAQ = codeSAQ;
        }

    }
}
