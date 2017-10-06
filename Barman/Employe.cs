using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class Employe
    {
        public virtual int? IdEmploye { get; set; }
        public virtual int? IdRole { get; set; }
        public virtual string Nom { get; set; }
        public virtual string CodeEmploye { get; set; }
        public virtual string Prenom { get; set; }
        public virtual string Telephone { get; set; }
        public virtual DateTime DateEmbauche { get; set; }
        public virtual string NAS { get; set; }

        public virtual Role SonRole { get; set; }

        public virtual IList<Vente> ListVente { get; set; } 

        public Employe()
        {
            IdEmploye = null;
            IdRole = null;
            Nom = String.Empty;
            CodeEmploye = String.Empty;
            Prenom = String.Empty;
            Telephone = String.Empty;
            DateEmbauche = DateTime.MinValue;
            NAS = String.Empty;

            SonRole = new Role();
            ListVente = new List<Vente>();
        }
    }
}
