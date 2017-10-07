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

        public Bouteille(string numero,int volumeInit,int volumeRestant,string etat,float prixB,int pIdEmplacement,int pIdMarque,int pIdCommande)
        {
            Numero = numero;
            VolumeInitial = volumeInit;
            VolumeRestant = volumeRestant;
            Etat = etat;
            PrixBouteille = prixB;
            IdEmplacement = pIdEmplacement;
            IdMarque = pIdMarque;
            IdCommande = pIdCommande;
        }
        public Bouteille(string numero, int volumeInit, int volumeRestant, string etat, float prixB, int pIdEmplacement, int pIdMarque, int pIdCommande,int pIdBouteille)
        {
            Numero = numero;
            VolumeInitial = volumeInit;
            VolumeRestant = volumeRestant;
            Etat = etat;
            PrixBouteille = prixB;
            IdEmplacement = pIdEmplacement;
            IdMarque = pIdMarque;
            IdCommande = pIdCommande;
            IdBouteille = pIdBouteille;
        }

        // Pour utiliser NHibernate, il faut surcharger Equals et GetHashCode.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Bouteille m = obj as Bouteille;

            if (m == null)
            {
                return false;
            }

            return this.IdBouteille == m.IdBouteille;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
