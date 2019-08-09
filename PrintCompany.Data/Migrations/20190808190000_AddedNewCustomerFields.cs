using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class AddedNewCustomerFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ContactDate",
                table: "OrderCustomerContacts",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Customers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ContactDate",
                table: "OrderCustomerContacts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
