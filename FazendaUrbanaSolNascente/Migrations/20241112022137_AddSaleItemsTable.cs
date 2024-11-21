using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FazendaUrbanaSolNascente.Migrations
{
    public partial class AddSaleItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Depositos_StorageId",
                table: "Produtos");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Produtos_productId",
                table: "Vendas");

            migrationBuilder.DropTable(
                name: "Depositos");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_productId",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_StorageId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "ProcutId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Produtos");

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => new { x.SaleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_SaleItems_Produtos_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleItems_Vendas_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.AddColumn<int>(
                name: "ProcutId",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Depositos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_productId",
                table: "Vendas",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_StorageId",
                table: "Produtos",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Depositos_StorageId",
                table: "Produtos",
                column: "StorageId",
                principalTable: "Depositos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Produtos_productId",
                table: "Vendas",
                column: "productId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
