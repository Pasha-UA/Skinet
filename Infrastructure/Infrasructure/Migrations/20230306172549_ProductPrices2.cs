using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItem_PriceItems_PriceItemId",
                table: "ProductPriceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItem_Products_ProductId",
                table: "ProductPriceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPriceItem",
                table: "ProductPriceItem");

            migrationBuilder.DropIndex(
                name: "IX_ProductPriceItem_ProductId",
                table: "ProductPriceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems");

            migrationBuilder.RenameTable(
                name: "ProductPriceItem",
                newName: "ProductPriceItems");

            migrationBuilder.RenameTable(
                name: "PriceItems",
                newName: "PriceItem");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPriceItem_PriceItemId",
                table: "ProductPriceItems",
                newName: "IX_ProductPriceItems_PriceItemId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductPriceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PriceItemId",
                table: "ProductPriceItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ProductPriceItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<long>(
                name: "PriceDate",
                table: "ProductPriceItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPriceItems",
                table: "ProductPriceItems",
                columns: new[] { "ProductId", "PriceItemId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceItems_Products_ProductId",
                table: "ProductPriceItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItems_PriceItem_PriceItemId",
                table: "ProductPriceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPriceItems_Products_ProductId",
                table: "ProductPriceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPriceItems",
                table: "ProductPriceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceItem",
                table: "PriceItem");

            migrationBuilder.DropColumn(
                name: "PriceDate",
                table: "ProductPriceItems");

            migrationBuilder.RenameTable(
                name: "ProductPriceItems",
                newName: "ProductPriceItem");

            migrationBuilder.RenameTable(
                name: "PriceItem",
                newName: "PriceItems");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPriceItems_PriceItemId",
                table: "ProductPriceItem",
                newName: "IX_ProductPriceItem_PriceItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ProductPriceItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PriceItemId",
                table: "ProductPriceItem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductPriceItem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPriceItem",
                table: "ProductPriceItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceItems",
                table: "PriceItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceItem_ProductId",
                table: "ProductPriceItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceItem_PriceItems_PriceItemId",
                table: "ProductPriceItem",
                column: "PriceItemId",
                principalTable: "PriceItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPriceItem_Products_ProductId",
                table: "ProductPriceItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
