using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class HibernateVenteService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Vente> RetrieveAll()
        {
            return session.Query<Vente>().ToList();
        }

        public static List<Vente> Retrieve(int pIdVente)
        {
            var ventes = session.Query<Vente>().AsQueryable();

            var result = from m in ventes
                         where m.IdVente == pIdVente
                         select m;

            return result.ToList();
        }

        public static void Create(Vente vente)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(vente);
                transaction.Commit();
            }
        }

        public static void Update(Vente vente)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(vente);
                transaction.Commit();
            }
        }

        public static void Delete(Vente vente)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(vente);
                transaction.Commit();
            }
        }

    }
}
