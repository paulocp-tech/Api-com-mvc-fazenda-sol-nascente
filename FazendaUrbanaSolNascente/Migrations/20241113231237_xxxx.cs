using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FazendaUrbanaSolNascente.Migrations
{
    public partial class xxxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_CustomerId",
                table: "Vendas",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Customers_CustomerId",
                table: "Vendas",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Customers_CustomerId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_CustomerId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Vendas");
        }
    }
}
