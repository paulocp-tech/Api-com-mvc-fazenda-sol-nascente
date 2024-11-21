using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FazendaUrbanaSolNascente.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalValue",
                table: "Vendas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Vendas");
        }
    }
}
