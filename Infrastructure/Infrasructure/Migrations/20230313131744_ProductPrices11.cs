using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Products_ProductId1",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId1",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductPrices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "ProductPrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId1",
                table: "ProductPrices",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Products_ProductId1",
                table: "ProductPrices",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
