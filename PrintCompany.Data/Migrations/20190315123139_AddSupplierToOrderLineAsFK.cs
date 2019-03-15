using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class AddSupplierToOrderLineAsFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "OrderLines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_SupplierId",
                table: "OrderLines",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_Suppliers_SupplierId",
                table: "OrderLines",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_Suppliers_SupplierId",
                table: "OrderLines");

            migrationBuilder.DropIndex(
                name: "IX_OrderLines_SupplierId",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "OrderLines");
        }
    }
}
