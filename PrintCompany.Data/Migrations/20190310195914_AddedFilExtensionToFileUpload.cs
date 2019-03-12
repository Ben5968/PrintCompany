using Microsoft.EntityFrameworkCore.Migrations;

namespace PrintCompany.Data.Migrations
{
    public partial class AddedFilExtensionToFileUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "FileUploads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "FileUploads");
        }
    }
}
