using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.EmplacementDossier.Hibernate
{
    public class EmplacementMap : ClassMap<Emplacement>
    {
        public EmplacementMap()
        {
            Table("emplacements");

            LazyLoad();

            Id(x => x.IdEmplacement)
             .Column("idEmplacement")
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
