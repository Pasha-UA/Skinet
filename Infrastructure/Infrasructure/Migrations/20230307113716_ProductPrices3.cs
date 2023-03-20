using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItems_PriceItem_PriceItemId",
                table: "ProductPriceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItem",
                table: "PriceItem");

            migrationBuilder.RenameTable(
                name: "PriceItem",
                newName: "PriceItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceItems_PriceItems_PriceItemId",
                table: "ProductPriceItems",
                column: "PriceItemId",
                principalTable: "PriceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItems_PriceItems_PriceItemId",
                table: "ProductPriceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems");

            migrationBuilder.RenameTable(
                name: "PriceItems",
                newName: "PriceItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceItem",
                table: "PriceItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceItems_PriceItem_PriceItemId",
                table: "ProductPriceItems",
                column: "PriceItemId",
                principalTable: "PriceItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
