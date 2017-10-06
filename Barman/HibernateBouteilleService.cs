using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class HibernateBouteilleService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Bouteille> RetrieveAll()
        {
            return session.Query<Bouteille>().ToList();
        }

        public static List<Bouteille> Retrieve(int pIdBouteille)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdBouteille == pIdBouteille
                         select m;

            return result.ToList();
        }

        public static void Create(Bouteille bouteille)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(bouteille);
                transaction.Commit();
            }
        }

        public static void Update(Bouteille bouteille)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(bouteille);
                transaction.Commit();
            }
        }

        public static void Delete(Bouteille bouteille)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(bouteille);
                transaction.Commit();
            }
        }

    }
}
