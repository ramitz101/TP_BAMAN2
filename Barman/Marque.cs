using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Marque
    {
        public virtual int? IdMarque { get; set; }
        public virtual int? IdTypeAlcool { get; set; }
        public virtual string Nom { get; set; }

        public virtual TypeAlcool SonTypeAlcool { get; set; }

        public Marque()
        {
            IdMarque = null;
            IdTypeAlcool = null;
            Nom = String.Empty;
            SonTypeAlcool = new TypeAlcool();
        }

        public Marque(string nom, int pIdTypeAlcool)
        {
            Nom = nom;
            IdTypeAlcool = pIdTypeAlcool;
            

        }
        public Marque(string nom, int pIdTypeAlcool,int pIdMarque)
        {
            Nom = nom;
            IdTypeAlcool = pIdTypeAlcool;
            IdMarque = pIdMarque;
        }

        // Pour utiliser NHibernate, il faut surcharger Equals et GetHashCode.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Marque m = obj as Marque;

            if (m == null)
            {
                return false;
            }

            return this.IdMarque == m.IdMarque;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
