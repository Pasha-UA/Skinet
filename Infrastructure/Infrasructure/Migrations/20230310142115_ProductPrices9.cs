using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_Currency_CurrencyId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_PriceType_PriceTypeId",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceType",
                table: "PriceType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.RenameTable(
                name: "PriceType",
                newName: "PriceTypes");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "Currencies");

            migrationBuilder.RenameIndex(
                name: "IX_PriceType_CurrencyId",
                table: "PriceTypes",
                newName: "IX_PriceTypes_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceTypes",
                table: "PriceTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceTypes_Currencies_CurrencyId",
                table: "PriceTypes",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceTypes_Currencies_CurrencyId",
                table: "PriceTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceTypes",
                table: "PriceTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "PriceTypes",
                newName: "PriceType");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currency");

            migrationBuilder.RenameIndex(
                name: "IX_PriceTypes_CurrencyId",
                table: "PriceType",
                newName: "IX_PriceType_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceType",
                table: "PriceType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceType_Currency_CurrencyId",
                table: "PriceType",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_PriceType_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");
        }
    }
}
