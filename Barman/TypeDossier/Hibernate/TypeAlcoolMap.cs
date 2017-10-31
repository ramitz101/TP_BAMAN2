using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.TypeDossier.Hibernate
{
    public class TypeAlcoolMap:ClassMap<TypeAlcool>
    {
        public TypeAlcoolMap()
        {
            Table("typesalcool");

            LazyLoad();

            Id(x => x.IdTypeAlcool)
             .Column("idTypeAlcool")
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
        }
    }
}
