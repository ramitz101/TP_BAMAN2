using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.TypeDossier
{
    public class TypeAlcool
    {
        public virtual int? IdTypeAlcool { get; set; }
        public virtual string Nom { get; set; }

        public TypeAlcool()
        {
            IdTypeAlcool = null;
            Nom = String.Empty;
        }

        public TypeAlcool(string nom)
        {
            Nom = nom;
        }

        public TypeAlcool(string nom,int pIdTypeAlcool)
        {
            Nom = nom;
            IdTypeAlcool = pIdTypeAlcool;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            TypeAlcool m = obj as TypeAlcool;

            if (m == null)
            {
                return false;
            }

            return this.IdTypeAlcool == m.IdTypeAlcool;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
