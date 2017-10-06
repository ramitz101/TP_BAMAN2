using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public static class HibernateTypeAlcoolService
    {
        private static ISession session = NHibernateConnexion.OpenSession();


        public static List<TypeAlcool> RetrieveAll()
        {
            return session.Query<TypeAlcool>().ToList();
        }

        public static List<TypeAlcool> retrieveTypeAlcool(int pIdTypeAlcool)
        {
            var type = session.Query<TypeAlcool>().AsQueryable();

            var resultat = from m in type
                           where m.IdTypeAlcool == pIdTypeAlcool
                           select m;
            return resultat.ToList();

        }

        public static void Create(TypeAlcool typeAlcool)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(typeAlcool);
                transaction.Commit();
            }
        }


        public static void Update(TypeAlcool typeAlcool)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(typeAlcool);
                transaction.Commit();
            }
        }

        public static void Delete(TypeAlcool typeAlcool)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(typeAlcool);
                transaction.Commit();
            }
        }


    }
}
