using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FazendaUrbanaSolNascente.Migrations
{
    public partial class AddPlantingAndProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plantings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plantings_Produtos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plantings_ProductId",
                table: "Plantings",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plantings");
        }
    }
}
