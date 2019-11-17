using Microsoft.EntityFrameworkCore.Migrations;

namespace prensaestudiantil.Web.Migrations
{
    public partial class UnifyUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "YoutubeVideos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Publications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_UserId",
                table: "YoutubeVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_UserId",
                table: "Publications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_AspNetUsers_UserId",
                table: "YoutubeVideos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications");

            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_AspNetUsers_UserId",
                table: "YoutubeVideos");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_UserId",
                table: "YoutubeVideos");

            migrationBuilder.DropIndex(
                name: "IX_Publications_UserId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Publications");
        }
    }
}
