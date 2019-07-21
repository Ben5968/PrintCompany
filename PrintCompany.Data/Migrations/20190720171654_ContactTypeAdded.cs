using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class ContactTypeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ContactDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactTypeId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ContactTypeId",
                table: "Orders",
                column: "ContactTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ContactTypes_ContactTypeId",
                table: "Orders",
                column: "ContactTypeId",
                principalTable: "ContactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ContactTypes_ContactTypeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ContactTypeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactTypeId",
                table: "Orders");
        }
    }
}
