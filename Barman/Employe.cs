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

        public Employe(string nom,string prenom,string telephone,string nas,DateTime dateEmbauche,string codeEmploye,int pIdRole)
        {
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            NAS = nas;
            DateEmbauche = dateEmbauche;
            CodeEmploye = codeEmploye;
            IdRole = pIdRole;
        }
        public Employe(string nom, string prenom, string telephone, string nas, DateTime dateEmbauche, string codeEmploye, int pIdRole,int pIdEmploye)
        {
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            NAS = nas;
            DateEmbauche = dateEmbauche;
            CodeEmploye = codeEmploye;
            IdRole = pIdRole;
            IdEmploye = pIdEmploye;
        }

        // Pour utiliser NHibernate, il faut surcharger Equals et GetHashCode.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Employe m = obj as Employe;

            if (m == null)
            {
                return false;
            }

            return this.IdEmploye == m.IdEmploye;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
