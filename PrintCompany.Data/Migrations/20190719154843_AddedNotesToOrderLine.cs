using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class AddedNotesToOrderLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OrderLines",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OrderLines");
        }
    }
}
