using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Currency_CurrencyId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prices",
                table: "Prices");

            migrationBuilder.RenameTable(
                name: "Prices",
                newName: "PriceType");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_ProductId",
                table: "PriceType",
                newName: "IX_PriceType_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_CurrencyId",
                table: "PriceType",
                newName: "IX_PriceType_CurrencyId");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "PriceType",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<long>(
                name: "DateTime",
                table: "PriceType",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PriceType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriceTypeId",
                table: "PriceType",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceType",
                table: "PriceType",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PriceType_PriceTypeId",
                table: "PriceType",
                column: "PriceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceType_Currency_CurrencyId",
                table: "PriceType",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceType_PriceType_PriceTypeId",
                table: "PriceType",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceType_Products_ProductId",
                table: "PriceType",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_Currency_CurrencyId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_PriceType_PriceTypeId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_Products_ProductId",
                table: "PriceType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceType",
                table: "PriceType");

            migrationBuilder.DropIndex(
                name: "IX_PriceType_PriceTypeId",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "PriceType");

            migrationBuilder.RenameTable(
                name: "PriceType",
                newName: "Prices");

            migrationBuilder.RenameIndex(
                name: "IX_PriceType_ProductId",
                table: "Prices",
                newName: "IX_Prices_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceType_CurrencyId",
                table: "Prices",
                newName: "IX_Prices_CurrencyId");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DateTime",
                table: "Prices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prices",
                table: "Prices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Currency_CurrencyId",
                table: "Prices",
                column: "CurrencyId",
                principalTable: "Currency",
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
