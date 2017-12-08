using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barman.BouteilleDossier;
using Barman.EmployeDossier;

namespace Barman.VenteDossier
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

        public virtual Bouteille laBouteille { get; set; }

        public virtual float Total { get { return (float)PrixOnce * (int)Volume; } set { Total = (float)PrixOnce * (int)Volume; } }
        public Vente()
        {
            IdVente = null;
            IdEmploye = null;
            IdBouteille = null;
            PrixOnce = null;
            DateVente = DateTime.MinValue;
            Volume = null;

            UnEmploye = new Employe();
            laBouteille = new Bouteille();
        }

        public Vente(float prixOnce,DateTime dateVente,int volume,int pIdBouteille,int pIdEmploye)
        {
            PrixOnce = prixOnce;
            DateVente = dateVente;
            Volume = volume;
            IdBouteille = pIdBouteille;
            IdEmploye = pIdEmploye;
        }
        public Vente(float prixOnce, DateTime dateVente, int? volume, int pIdBouteille, int pIdEmploye,int pIdVente)
        {
            PrixOnce = prixOnce;
            DateVente = dateVente;
            Volume = volume;
            IdBouteille = pIdBouteille;
            IdEmploye = pIdEmploye;
            IdVente = pIdVente;
        }

        

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Vente m = obj as Vente;

            if (m == null)
            {
                return false;
            }

            return this.IdVente == m.IdVente;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
