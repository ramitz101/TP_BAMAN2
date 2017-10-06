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
    }
}
