using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public static class HibernateEmplacementService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Emplacement> RetrieveAll()
        {
            return session.Query<Emplacement>().ToList();
        }

        public static List<Emplacement> retrieveEmplacement(int pIdEmplacement)
        {
            var emplacements = session.Query<Emplacement>().AsQueryable();

            var resultat = from m in emplacements
                           where m.IdEmplacement == pIdEmplacement
                           select m;
            return resultat.ToList();

        }
      public static List<Emplacement> retrieveEmplacementByNom(string pNomEmplacement)
      {
         var emplacements = session.Query<Emplacement>().AsQueryable();

         var resultat = from m in emplacements
                        where m.Nom == pNomEmplacement
                        select m;
         return resultat.ToList();

      }

      public static void Create(Emplacement emplacement)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(emplacement);
                transaction.Commit();
            }
        }


        public static void Update(Emplacement emplacement)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(emplacement);
                transaction.Commit();
            }
        }

        public static void Delete(Emplacement emplacement)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(emplacement);
                transaction.Commit();
            }
        }


    }
}
