using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.TypeDossier.Hibernate
{
    public static class HibernateTypeAlcoolService
    {
        private static ISession session = NHibernateConnexion.OpenSession();


        public static List<TypeAlcool> RetrieveAll()
        {
            return session.Query<TypeAlcool>().ToList();
        }

        public static List<TypeAlcool> RetrieveTypeAlcool(int pIdTypeAlcool)
        {
            var typeAlcooles = session.Query<TypeAlcool>().AsQueryable();

            var resultat = from m in typeAlcooles
                           where m.IdTypeAlcool == pIdTypeAlcool
                           select m;
            return resultat.ToList();

        }
        public static List<string> RetrieveAllTypeAlcool()
        {
            var typeAlcooles = session.Query<TypeAlcool>().AsQueryable();

            var resultat = from m in typeAlcooles                           
                           select m.Nom;
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
