using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Knapcode.PoGoNotifications.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PokemonEncounters",
                columns: table => new
                {
                    EncounterId = table.Column<string>(nullable: false),
                    DisappearTime = table.Column<DateTimeOffset>(nullable: false),
                    SpawnpointId = table.Column<string>(nullable: true),
                    PokemonId = table.Column<int>(nullable: false),
                    IsLured = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonEncounters", x => new { x.EncounterId, x.DisappearTime });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonEncounters");
        }
    }
}
