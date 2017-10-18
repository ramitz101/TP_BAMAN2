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
