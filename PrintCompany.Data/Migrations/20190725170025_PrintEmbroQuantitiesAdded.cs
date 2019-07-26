using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class PrintEmbroQuantitiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmbroideryQuantity",
                table: "OrderLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrintQuantity",
                table: "OrderLines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbroideryQuantity",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "PrintQuantity",
                table: "OrderLines");
        }
    }
}
