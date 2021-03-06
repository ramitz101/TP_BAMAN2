﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barman.VenteDossier.Hibernate
{
    class VenteMap:ClassMap<Vente>
    {
        public VenteMap()
        {
            Table("ventes");

            LazyLoad();


            Id(x => x.IdVente)
                .Column("idVente")
                .CustomType<int?>()
                .Access.Property()
                .CustomSqlType("INTEGER")
                .Not.Nullable()
                .GeneratedBy.Identity();

            Map(x => x.DateVente)
             .Column("dateVente")
             .CustomType<DateTime>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("datetime");

            Map(x => x.PrixOnce)
             .Column("prixOnce")
             .CustomType<float?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("decimal");

            Map(x => x.Volume)
             .Column("volume")
             .CustomType<int?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("INTEGER");

            Map(x => x.IdEmploye)
             .Column("idEmploye")
             .CustomType<int?>()
             .Access.Property()
             .Generated.Never()
             .CustomSqlType("INTEGER");

            Map(x => x.IdBouteille)
           .Column("idBouteille")
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

            //References(x => x.laBouteille)
            //    .Class<Bouteille>()
            //    .Access.Property()
            //    .LazyLoad(Laziness.False)
            //    .Cascade.None()
            //    .Columns("idBouteille");


        }
    }
}
