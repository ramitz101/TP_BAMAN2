using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barman.MarqueDossier;
using Barman.EmplacementDossier.Hibernate;

namespace Barman.BouteilleDossier.Hibernate
{
    public class HibernateBouteilleService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        
        /// <summary>
        /// Mettre sup a true pour avoir les bouteilles supprimer
        /// </summary>
        /// <param name="sup"></param>
        /// <returns></returns>
        public static List<Bouteille> RetrieveAll(bool? sup)
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         select b;
            if (sup == false)
            {
                result = from b in bouteilles
                         where b.Etat != "Perdue"
                         select b;
            }
            else
            {
                result = from b in bouteilles
                         where b.Etat == "Perdue"
                         select b;
            }
            return result.ToList();
        }

        public static int CountNbEntamee()
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         where b.Etat == "Entamée"
                         select b;

            return result.ToList().Count;
        }
        public static int CountNbPleine()
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         where b.Etat == "Pleine"
                         select b;

            return result.ToList().Count;
        }

        public static int CountNbReserve()
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         where b.IdEmplacement==HibernateEmplacementService.Retrieve("Réserve")[0].IdEmplacement
                         select b;

            return result.ToList().Count;
        }
        public static int CountNbPerdue()
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         where b.Etat=="Perdue"
                         select b;

            return result.ToList().Count;
        }


        public static List<Bouteille> RetrieveAllPerdue()
        {
            var bouteilles = session.Query<Bouteille>().ToList();

            var result = from b in bouteilles
                         where b.Etat == "Perdue"
                         select b;

            return result.ToList();
        }

        public static List<int?> RetrieveIdMarqueEnReserve()
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

            var result = from m in bouteilles
                         where m.IdEmplacement == (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement && m.Etat != "Supprimée"
                         select m.IdMarque;
            return result.ToList();
        }
        public static List<Bouteille> RetrieveByMarque(Marque pMarque)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.IdMarque == pMarque.IdMarque && m.IdEmplacement == (int)HibernateEmplacementService.retrieveEmplacementByNom("Réserve")[0].IdEmplacement
                      select m;

            return result.ToList();
         }

        
        public static List<Bouteille> RetrieveByEtat(string s, bool? sup)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();
            var result = from m in bouteilles
                         select m;

            if (sup == false)
            {
                 result = from m in bouteilles
                             where m.Etat.StartsWith(s) && m.Etat != "Perdue"
                          select m;
            }
            else
            {
                result = from m in bouteilles
                         where m.Etat.StartsWith(s) && m.Etat == "Perdue"
                         select m;
            }
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

     


        public static List<Bouteille> RetrieveByMarqueId(int pIdMarque, bool? sup)
      {
            
         var bouteilles = session.Query<Bouteille>().AsQueryable();
            var result = from m in bouteilles
                         select m;
            if (sup==false)
            {
                 result = from m in bouteilles
                             where m.IdMarque == pIdMarque && m.Etat != "Perdue"
                          select m;
            }
            else
            {
                 result = from m in bouteilles
                             where m.IdMarque == pIdMarque && m.Etat == "Perdue"
                          select m;
            }
         

         return result.ToList();
      }
      public static List<Bouteille>  RetrieveByEmplacementId(int pIdEmplacement,bool? sup)
        {
            var bouteilles = session.Query<Bouteille>().AsQueryable();
            var result = from m in bouteilles
                         select m;
            if(sup==false)
            {
                result = from m in bouteilles
                         where m.IdEmplacement == pIdEmplacement && m.Etat != "Perdue"
                         select m;
            }
            else
            {
                result = from m in bouteilles
                         where m.IdEmplacement == pIdEmplacement && m.Etat == "Perdue"
                         select m;
            }
             
            
            return result.ToList();
        }
      public static List<Bouteille> Retrieve(string pNomMarque)
      {
         var bouteilles = session.Query<Bouteille>().AsQueryable();

         var result = from m in bouteilles
                      where m.SaMarque.Nom.StartsWith(pNomMarque) && m.Etat != "Perdue"
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
