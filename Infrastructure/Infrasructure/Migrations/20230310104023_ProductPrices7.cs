using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class ProductPrices7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_PriceType_PriceTypeId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_Products_ProductId",
                table: "PriceType");

            migrationBuilder.DropIndex(
                name: "IX_PriceType_PriceTypeId",
                table: "PriceType");

            migrationBuilder.DropIndex(
                name: "IX_PriceType_ProductId",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PriceType");

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true),
                    PriceTypeId = table.Column<string>(type: "TEXT", nullable: true),
                    DateTime = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_PriceType_PriceTypeId",
                        column: x => x.PriceTypeId,
                        principalTable: "PriceType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PriceTypeId",
                table: "Prices",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductId",
                table: "Prices",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.AddColumn<long>(
                name: "DateTime",
                table: "PriceType",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "PriceType",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "PriceType",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceType_PriceTypeId",
                table: "PriceType",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceType_ProductId",
                table: "PriceType",
                column: "ProductId");

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
    }
}
