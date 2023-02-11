using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.data.migrations
{
    public partial class CategoryTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrands_ProductCategoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_ProductCategoryId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrands_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId",
                principalTable: "ProductBrands",
                principalColumn: "Id");
        }
    }
}
