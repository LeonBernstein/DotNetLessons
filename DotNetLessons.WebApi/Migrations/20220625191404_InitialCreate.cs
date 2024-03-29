﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetLessons.WebApi.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Persons",
            columns: table => new
            {
                PersonId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsFromEarth = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Persons", x => x.PersonId);
            });

        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                AddressId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PersonId = table.Column<int>(type: "int", nullable: false),
                GalaxyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PlanetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HasCosmicRadiation = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.AddressId);
                table.ForeignKey(
                    name: "FK_Addresses_Persons_PersonId",
                    column: x => x.PersonId,
                    principalTable: "Persons",
                    principalColumn: "PersonId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_PersonId",
            table: "Addresses",
            column: "PersonId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Addresses");

        migrationBuilder.DropTable(
            name: "Persons");
    }
}
