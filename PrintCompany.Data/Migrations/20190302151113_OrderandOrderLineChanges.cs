using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class OrderandOrderLineChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbroideryRequired",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrintRequired",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ItemCount",
                table: "OrderLines",
                newName: "Quantity");

            migrationBuilder.AddColumn<bool>(
                name: "EmbroideryRequired",
                table: "OrderLines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrintRequired",
                table: "OrderLines",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbroideryRequired",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "PrintRequired",
                table: "OrderLines");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderLines",
                newName: "ItemCount");

            migrationBuilder.AddColumn<bool>(
                name: "EmbroideryRequired",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrintRequired",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }
    }
}
