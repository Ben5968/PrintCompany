using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class NewUserRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbroideryQuantityCompletedTotalByOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EmbroideryQuantityTotalByOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTotalQuantity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrintQuantityCompletedTotalByOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrintQuantityTotalByOrder",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmbroideryQuantityCompletedTotalByOrder",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmbroideryQuantityTotalByOrder",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderTotalQuantity",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrintQuantityCompletedTotalByOrder",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrintQuantityTotalByOrder",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }
    }
}
