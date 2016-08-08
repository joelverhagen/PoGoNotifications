using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Migrations
{
    [DbContext(typeof(NotificationContext))]
    [Migration("20160808000422_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Knapcode.PoGoNotifications.Models.PokemonEncounter", b =>
                {
                    b.Property<string>("EncounterId");

                    b.Property<DateTimeOffset>("DisappearTime");

                    b.Property<string>("SpawnpointId");

                    b.Property<int>("PokemonId");

                    b.Property<bool>("IsLured");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("EncounterId", "DisappearTime", "SpawnpointId", "PokemonId");

                    b.ToTable("PokemonEncounters");
                });
        }
    }
}
