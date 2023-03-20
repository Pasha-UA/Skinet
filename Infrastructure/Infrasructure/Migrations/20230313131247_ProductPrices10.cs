using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices");

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "ProductPrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId1",
                table: "ProductPrices",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_PriceTypes_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId",
                principalTable: "PriceTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Products_ProductId1",
                table: "ProductPrices",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_PriceTypes_PriceTypeId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Products_ProductId1",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId1",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductPrices");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Products_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
