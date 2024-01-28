using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrasructure.Migrations
{
    public partial class DeletePropModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
