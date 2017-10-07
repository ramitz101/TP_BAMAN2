using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Emplacement
    {
        public virtual int? IdEmplacement { get; set; }
        public virtual string Nom { get; set; }

        public Emplacement()
        {
            IdEmplacement = null;
            Nom = String.Empty;
        }
        public Emplacement(string nom)
        {
            Nom = nom;
        }
        public Emplacement(string nom,int pIdEmplacement)
        {
            Nom = nom;
            IdEmplacement = pIdEmplacement;
        }
        // Pour utiliser NHibernate, il faut surcharger Equals et GetHashCode.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Emplacement m = obj as Emplacement;

            if (m == null)
            {
                return false;
            }

            return this.IdEmplacement == m.IdEmplacement;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
