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

        public static List<Bouteille> RetrieveByMarque(Marque pMarque)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.IdMarque == pMarque.IdMarque && m.IdEmplacement == (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement
                      select m;

            return result.ToList();
         }
        public static List<Bouteille> Retrieve(int pIdBouteille)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdBouteille == pIdBouteille
                         select m;

            return result.ToList();
        }
      public static List<Bouteille> RetrieveByMarqueId(int pIdMarque)
      {
         var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.IdMarque == pIdMarque
                      select m;

         return result.ToList();
      }
      public static List<Bouteille> Retrieve(string pNomMarque)
      {
         var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.SaMarque.Nom.StartsWith(pNomMarque)
                      select m;

         return result.ToList();
      }

        // Retrouver toute les bouteilles avec le même idCommande
        public static List<Bouteille> RetrieveByIdCommande(int pIdCommande)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdCommande == pIdCommande
                         select m;

            return result.ToList();
        }


        //Retrouver une bouteille par son idmarque et son volumeInitial
        public static List<Bouteille> RetrieveByMarqueEtVolInitial(int pIdMarque,int v)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdMarque == pIdMarque && m.VolumeInitial == v 
                         select m;

            return result.ToList();
        }


        public static List<Bouteille> RetrieveByUnique(int pIdMarque, int pIdEmplacement, string pNumero)
      {
         var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.IdMarque==pIdMarque&&m.IdEmplacement==pIdEmplacement&& m.Numero==pNumero
                      select m;

         return result.ToList();
      }


       

        // Retrouver toute les bouteilles qui sont a un emplacement
        public static List<Bouteille> RetrieveBouteilleEmplacement(int pIdEmplacement)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdEmplacement == pIdEmplacement
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
