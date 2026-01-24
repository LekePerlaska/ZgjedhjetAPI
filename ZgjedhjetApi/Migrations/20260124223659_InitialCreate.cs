using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZgjedhjetApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zgjedhjet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Komuna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qendra_e_votimit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendvotimi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Partia_Partia111 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia112 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia113 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia114 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia115 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia116 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia117 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia118 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia119 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia120 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia121 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia122 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia123 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia124 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia125 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia126 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia127 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia128 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia129 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia130 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia131 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia132 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia133 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia134 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia135 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia136 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia137 = table.Column<int>(type: "int", nullable: false),
                    Partia_Partia138 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zgjedhjet", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zgjedhjet");
        }
    }
}
