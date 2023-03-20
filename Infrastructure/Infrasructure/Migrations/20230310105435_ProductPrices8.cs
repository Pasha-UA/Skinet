using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_PriceType_PriceTypeId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prices",
                table: "Prices");

            migrationBuilder.RenameTable(
                name: "Prices",
                newName: "ProductPrices");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_ProductId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_PriceTypeId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_PriceTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_PriceType_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_PriceType_PriceTypeId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "Prices");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductId",
                table: "Prices",
                newName: "IX_Prices_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "Prices",
                newName: "IX_Prices_PriceTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prices",
                table: "Prices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_PriceType_PriceTypeId",
                table: "Prices",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
