using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman
{
    public class MarqueMap: ClassMap<Marque>
    {
        public MarqueMap()
        {
            Table("marques");

            LazyLoad();

            Id(x => x.IdMarque)
             .Column("idMarque")
             .CustomType<int?>()
             .Access.Property()
             .CustomSqlType("INTEGER")
             .Not.Nullable()
             .GeneratedBy.Identity();

            Map(x => x.Nom)
             .Column("nom")
             .CustomType<string>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("VARCHAR");

            // FOREING KEY
            Map(x => x.IdTypeAlcool)
             .Column("idTypeAlcool")
             .CustomType<int?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("INTEGER");

            // Autre class
            //References(x => x.SonTypeAlcool)
            //    .Class<TypeAlcool>()
            //    .Access.Property()
            //    .LazyLoad(Laziness.False)
            //    .Cascade.None()
            //    .Columns("idTypeAlcool");
        }
    }
}
