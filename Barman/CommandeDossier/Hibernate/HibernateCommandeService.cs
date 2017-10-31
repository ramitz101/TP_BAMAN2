using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.CommandeDossier.Hibernate
{
    class HibernateCommandeService
    {

        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Commande> RetrieveAll()
        {
            return session.Query<Commande>().ToList();
        }

        public static List<Commande> Retrieve(int pIdCommande)
        {
            var commandes = session.Query<Commande>().AsQueryable();

            var result = from m in commandes
                         where m.IdCommande == pIdCommande
                         select m;

            return result.ToList();
        }

        public static void Create(Commande commande)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(commande);
                transaction.Commit();
            }
        }

        public static void Update(Commande commande)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(commande);
                transaction.Commit();
            }
        }

        public static void Delete(Commande commande)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(commande);
                transaction.Commit();
            }
        }

    }
}
