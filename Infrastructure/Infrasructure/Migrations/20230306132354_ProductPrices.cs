using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceItems_Products_ProductId",
                table: "PriceItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceItems_ProductId",
                table: "PriceItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PriceItems");

            migrationBuilder.CreateTable(
                name: "ProductPriceItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true),
                    PriceItemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPriceItem_PriceItems_PriceItemId",
                        column: x => x.PriceItemId,
                        principalTable: "PriceItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductPriceItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceItem_PriceItemId",
                table: "ProductPriceItem",
                column: "PriceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceItem_ProductId",
                table: "ProductPriceItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPriceItem");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "PriceItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceItems_ProductId",
                table: "PriceItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceItems_Products_ProductId",
                table: "PriceItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
