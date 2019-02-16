using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class OrderLineAddedForeignKeyProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemColors_ItemColorId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemSizes_ItemSizeId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemTypes_ItemTypeId",
                table: "OrderLines");

            migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "OrderLines",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemSizeId",
                table: "OrderLines",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemColorId",
                table: "OrderLines",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemColors_ItemColorId",
                table: "OrderLines",
                column: "ItemColorId",
                principalTable: "ItemColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemSizes_ItemSizeId",
                table: "OrderLines",
                column: "ItemSizeId",
                principalTable: "ItemSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemTypes_ItemTypeId",
                table: "OrderLines",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemColors_ItemColorId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemSizes_ItemSizeId",
                table: "OrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLines_ItemTypes_ItemTypeId",
                table: "OrderLines");

            migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "OrderLines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ItemSizeId",
                table: "OrderLines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ItemColorId",
                table: "OrderLines",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemColors_ItemColorId",
                table: "OrderLines",
                column: "ItemColorId",
                principalTable: "ItemColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemSizes_ItemSizeId",
                table: "OrderLines",
                column: "ItemSizeId",
                principalTable: "ItemSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLines_ItemTypes_ItemTypeId",
                table: "OrderLines",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
