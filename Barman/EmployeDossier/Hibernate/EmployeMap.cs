using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barman.VenteDossier;
using Barman.RoleDossier;

namespace Barman.EmployeDossier.Hibernate
{
    public class EmployeMap : ClassMap<Employe>
    {
        public EmployeMap()
        {
            Table("employes");

            LazyLoad();

            Id(x => x.IdEmploye)
             .Column("idEmploye")
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

            Map(x => x.Prenom)
            .Column("prenom")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            Map(x => x.Etat)
            .Column("etat")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            Map(x => x.Telephone)
            .Column("telephone")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            Map(x => x.CodeEmploye)
            .Column("codeEmploye")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            Map(x => x.NAS)
            .Column("NAS")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            Map(x => x.DateEmbauche)
            .Column("dateEmbauche")
            .CustomType<DateTime>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("date");

            Map(x => x.IdRole)
             .Column("idRole")
             .CustomType<int?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("INTEGER");

            //Autre class
            References(x => x.SonRole)
                 .Class<Role>()
                 .Access.Property()
                 .LazyLoad(Laziness.False)
                 .Cascade.None()
                 .Columns("idRole");


            HasMany<Vente>(x => x.ListVente)
                .Not.LazyLoad()
                .Access.Property()
                .Cascade.All()
                .KeyColumns.Add("idEmploye", map => map.Name("idEmploye")
                                                    .SqlType("INTEGER")
                                                    .Nullable());


        }
    }
}
