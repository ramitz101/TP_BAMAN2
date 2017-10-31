using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Barman.CommandeDossier.Hibernate
{
    public class CommandeMap: ClassMap<Commande>
    {
        public CommandeMap()
        {
            Table("commandes");

            LazyLoad();

            Id(x => x.IdCommande)
             .Column("idCommande")
             .CustomType<int?>()
             .Access.Property()
             .CustomSqlType("INTEGER")
             .Not.Nullable()
             .GeneratedBy.Identity();

            Map(x => x.DateCommande)
             .Column("dateCommande")
             .CustomType<DateTime>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("datetime");

            Map(x => x.Etat)
          .Column("Etat")
          .CustomType<string>()
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("VARCHAR");

            // Foreign key
            Map(x => x.IdEmploye)
             .Column("idEmploye")
             .CustomType<int?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("INTEGER");


            // Autre class
            //References(x => x.UnEmploye)
            //    .Class<Employe>()
            //    .Access.Property()
            //    .LazyLoad(Laziness.False)
            //    .Cascade.None()
            //    .Columns("idEmploye");

            //HasMany<Bouteille>(x => x.ListBouteille)
            //.Not.LazyLoad()
            //.Access.Property()
            //.Cascade.All()
            //.KeyColumns.Add("idCommande", map => map.Name("idCommande")
            //                                    .SqlType("INTEGER")
            //                                    .Nullable());

        }
    }
}
