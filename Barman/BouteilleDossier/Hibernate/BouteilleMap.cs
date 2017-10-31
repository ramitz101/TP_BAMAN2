using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Barman.BouteilleDossier.Hibernate
{
    public class BouteilleMap : ClassMap<Bouteille>
    {
        public BouteilleMap()
        {
            Table("bouteilles");

            LazyLoad();

            Id(x => x.IdBouteille)
                .Column("idBouteille")
                .CustomType<int?>()
                .Access.Property()
                .CustomSqlType("INTEGER")
                .Not.Nullable()
                .GeneratedBy.Identity();

            Map(x => x.Numero)
             .Column("numero")
             .CustomType<string>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("VARCHAR");

            Map(x => x.PrixBouteille)
            .Column("prixBouteille")
            .CustomType<float?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("DECIMAL");

            Map(x => x.VolumeInitial)
            .Column("volumeInitial")
            .CustomType<int?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("INTEGER");

            Map(x => x.VolumeRestant)
            .Column("volumeRestant")
            .CustomType<int?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("INTEGER");

            Map(x => x.Etat)
            .Column("etat")
            .CustomType<string>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("VARCHAR");

            // FOREING KEY
            Map(x => x.IdCommande)
            .Column("idCommande")
            .CustomType<int?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("INTEGER");

            Map(x => x.IdEmplacement)
            .Column("idEmplacement")
            .CustomType<int?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("INTEGER");

            Map(x => x.IdMarque)
            .Column("idMarque")
            .CustomType<int?>()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("INTEGER");

            // Autre class
            //References(x => x.SaMarque)
            //    .Class<Marque>()
            //    .Access.Property()
            //    .LazyLoad(Laziness.False)
            //    .Cascade.None()
            //    .Columns("idMarque");

            //References(x => x.SonEmplacement)
            //    .Class<Emplacement>()
            //    .Access.Property()
            //    .LazyLoad(Laziness.False)
            //    .Cascade.None()
            //    .Columns("idEmplacement");




        }
    }
}
