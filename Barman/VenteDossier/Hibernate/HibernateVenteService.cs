using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.VenteDossier.Hibernate
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

        //Retrouver toutes les ventes faites par un seul employé pour une date
        public static List<Vente> RetrieveVenteEmploye(int pIdEmploye,DateTime pdate)
        {
            var ventes = session.Query<Vente>().AsQueryable();

            var result = from m in ventes
                         where m.IdEmploye == pIdEmploye && m.DateVente.Date == pdate.Date
                         select m;

            return result.ToList();
        }

        //Retrouver toutes les ventes faites par un seul employé
        public static List<Vente> RetrieveAllVenteEmploye(int pIdEmploye)
        {
            var ventes = session.Query<Vente>().AsQueryable();

            var result = from m in ventes
                         where m.IdEmploye == pIdEmploye
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
