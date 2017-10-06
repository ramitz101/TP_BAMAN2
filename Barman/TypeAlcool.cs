using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class TypeAlcool
    {
        public virtual int? IdType { get; set; }
        public virtual string Nom { get; set; }

        public TypeAlcool()
        {
            IdType = null;
            Nom = String.Empty;
        }
    }
}
