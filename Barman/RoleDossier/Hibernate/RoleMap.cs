using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.RoleDossier.Hibernate
{
    public class RoleMap: ClassMap<Role>
    {
        public RoleMap()
        {
            Table("roles");

            LazyLoad();

            Id(x => x.IdRole)
            .Column("idRole")
            .CustomType<int?>()
            .Access.Property()
            .CustomSqlType("INTEGER")
            .Not.Nullable()
            .GeneratedBy.Identity();

            Map(x => x.Code)
             .Column("codeRole")
             .CustomType<string>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("VARCHAR");
        }
    }
}
