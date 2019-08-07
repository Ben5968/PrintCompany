using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class NewOrderFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ColectionDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CollectionNote",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RelatedOrderExists",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RelatedOrderNote",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColectionDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CollectionNote",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RelatedOrderExists",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RelatedOrderNote",
                table: "Orders");
        }
    }
}
