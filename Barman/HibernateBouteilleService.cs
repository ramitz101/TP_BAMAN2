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


    }
}
