using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public static class HibernateMarqueService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Marque> RetrieveAll()
        {
            return session.Query<Marque>().ToList();
        }

        public static List<Marque> Retrieve(int pIdMarque)
        {
            var marques = session.Query<Marque>().AsQueryable();

            var result = from m in marques
                         where m.IdMarque == pIdMarque
                         select m;

            return result.ToList();
        }
      public static List<Marque> Retrieve(string pNom)
      {
         var marques = session.Query<Marque>().AsQueryable();

         var result = from m in marques
                      where m.Nom.StartsWith(pNom)
                      select m;

         return result.ToList();
      }

      public static List<Marque>RetrieveByType(TypeAlcool pType)
      {
         var marques = session.Query<Marque>().AsQueryable();

         var result = from m in marques
                      where m.IdTypeAlcool == pType.IdTypeAlcool
                      select m;

         return result.ToList();
      }


        public static void Create(Marque marque)
        {
            using (var transaction = session.BeginTransaction())
            {
               
                session.Save(marque);
                transaction.Commit();
            }
        }

        public static void Update(Marque marque)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(marque);
                transaction.Commit();
            }
        }

        public static void Delete(Marque marque)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(marque);
                transaction.Commit();
            }
        }
    }
}
