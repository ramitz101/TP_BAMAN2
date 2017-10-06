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
    }
}
