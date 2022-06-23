using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetLessons.WebApi.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                AddressId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                GalaxyName = table.Column<string>(type: "TEXT", nullable: false),
                PlanetName = table.Column<string>(type: "TEXT", nullable: false),
                HasConsmicRadiation = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.AddressId);
            });

        migrationBuilder.CreateTable(
            name: "Persons",
            columns: table => new
            {
                PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                FirstName = table.Column<string>(type: "TEXT", nullable: false),
                LastName = table.Column<string>(type: "TEXT", nullable: false),
                IsFromEarth = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Persons", x => x.PersonId);
                table.ForeignKey(
                    name: "FK_Persons_Addresses_PersonId",
                    column: x => x.PersonId,
                    principalTable: "Addresses",
                    principalColumn: "AddressId",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Persons");

        migrationBuilder.DropTable(
            name: "Addresses");
    }
}
