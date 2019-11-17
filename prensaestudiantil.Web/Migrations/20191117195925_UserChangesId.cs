using Microsoft.EntityFrameworkCore.Migrations;

namespace prensaestudiantil.Web.Migrations
{
    public partial class UserChangesId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MUserId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_MUserId",
                table: "AspNetUsers",
                column: "MUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_MUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MUserId",
                table: "AspNetUsers");
        }
    }
}
