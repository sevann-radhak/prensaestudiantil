using Microsoft.EntityFrameworkCore.Migrations;

namespace prensaestudiantil.Web.Migrations
{
    public partial class CategoryNameRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PublicationCategories",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PublicationCategories",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
