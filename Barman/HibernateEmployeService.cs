using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    class HibernateEmployeService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Employe> RetrieveAll()
        {
            return session.Query<Employe>().ToList();
        }

        public static List<Employe> Retrieve(int pIdEmploye)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.IdEmploye == pIdEmploye
                         select m;

            return result.ToList();
        }

        public static List<Employe> RetrieveNom(string pNom)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.Nom.StartsWith(pNom)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrievePrenom(string pPrenom)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.Prenom.StartsWith(pPrenom)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrieveNAS(string pNAS)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.NAS.StartsWith(pNAS)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrieveTelephone(string pTelephone)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.Telephone.StartsWith(pTelephone)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrieveDateEmbauche(string pDateEmbauche)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.DateEmbauche.ToString().Contains(pDateEmbauche)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrieveCode(string pCode)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.CodeEmploye.StartsWith(pCode)
                         select m;

            return result.ToList();
        }
        public static List<Employe> RetrieveRole(int pIdRole)
        {
            var employes = session.Query<Employe>().AsQueryable();

            var result = from m in employes
                         where m.IdRole == pIdRole
                         select m;

            return result.ToList();
        }



        public static List<string> RetrieveAllCodeEmploye()
        {
            var employes = session.Query<Employe>().ToList();

            var result = from m in employes               
                         select m.CodeEmploye;

            return result.ToList();
        }

        public static void Create(Employe employe)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(employe);
                transaction.Commit();
            }
        }

        public static void Update(Employe employe)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(employe);
                transaction.Commit();
            }
        }

        public static void Delete(Employe employe)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(employe);
                transaction.Commit();
            }
        }

    }
}
