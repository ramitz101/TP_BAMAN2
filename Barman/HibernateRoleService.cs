using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class HibernateRoleService
    {
        private static ISession session = NHibernateConnexion.OpenSession();

        public static List<Role> RetrieveAll()
        {
            return session.Query<Role>().ToList();
        }

        public static List<Role> Retrieve(int pIdRole)
        {
            var roles = session.Query<Role>().AsQueryable();

            var result = from m in roles
                         where m.IdRole == pIdRole
                         select m;

            return result.ToList();
        }
        public static List<Role> Retrieve(string pNomRole)
        {
            var roles = session.Query<Role>().AsQueryable();

            var result = from m in roles
                         where m.Code == pNomRole
                         select m;

            return result.ToList();
        }
        public static List<Role> RetrieveRole(string pNomRole)
        {
            var roles = session.Query<Role>().AsQueryable();

            var result = from m in roles
                         where m.Code.StartsWith(pNomRole)
                         select m;

            return result.ToList();


        }

            public static void Create(Role role)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(role);
                transaction.Commit();
            }
        }

        public static void Update(Role role)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Update(role);
                transaction.Commit();
            }
        }

        public static void Delete(Role role)
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(role);
                transaction.Commit();
            }
        }
    }
}
