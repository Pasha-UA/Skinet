using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class pricesfieldadd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItem",
                table: "PriceItem");

            migrationBuilder.DropColumn(
                name: "PriceItemId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "PriceItem",
                newName: "PriceItems");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "PriceItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceItems_Products_ProductId",
                table: "PriceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceItems_ProductId",
                table: "PriceItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PriceItems");

            migrationBuilder.RenameTable(
                name: "PriceItems",
                newName: "PriceItem");

            migrationBuilder.AddColumn<string>(
                name: "PriceItemId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceItem",
                table: "PriceItem",
                column: "Id");
        }
    }
}
