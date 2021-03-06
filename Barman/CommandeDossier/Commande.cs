﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barman.BouteilleDossier;
using Barman.EmployeDossier;

namespace Barman.CommandeDossier
{
    public class Commande
    {
        public virtual int? IdCommande { get; set; }
        public virtual int? IdEmploye { get; set; }

        public virtual DateTime DateCommande { get; set; }

        public virtual Employe UnEmploye { get; set; }

        public virtual string Etat { get; set; }

        public virtual IList<Bouteille> ListBouteille { get; set; }
        //public int Quantite { get; set; }

        public Commande()
        {
            IdCommande = null;
            IdEmploye = null;
            Etat = string.Empty;
            DateCommande = DateTime.MinValue;
            UnEmploye = new Employe();
            ListBouteille = new List<Bouteille>();
        }
        public Commande(DateTime dateCommande,int pIdEmploye)
        {
            DateCommande = dateCommande;
            IdEmploye = pIdEmploye;
        }
        public Commande(DateTime dateCommande, int pIdEmploye, int? pIdCommande,string etat)
        {
            DateCommande = dateCommande;
            IdEmploye = pIdEmploye;
            IdCommande = pIdCommande;
            Etat = etat;
        }

        public Commande(Commande c)
        {
            IdCommande = c.IdCommande;
            IdEmploye = c.IdEmploye;
            DateCommande = c.DateCommande;
            UnEmploye = c.UnEmploye;
            ListBouteille = c.ListBouteille;
        }

        // Pour utiliser NHibernate, il faut surcharger Equals et GetHashCode.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Commande m = obj as Commande;

            if (m == null)
            {
                return false;
            }

            return this.IdCommande == m.IdCommande;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
