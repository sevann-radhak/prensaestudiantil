using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prensaestudiantil.Web.Migrations
{
    public partial class PublicationsWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Writers_WriterId",
                table: "Publications");

            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_Writers_WriterId",
                table: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Writers");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_WriterId",
                table: "YoutubeVideos");

            migrationBuilder.DropIndex(
                name: "IX_Publications_WriterId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Publications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "YoutubeVideos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Publications",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_WriterId",
                table: "YoutubeVideos",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_WriterId",
                table: "Publications",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Writers_UserId",
                table: "Writers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Writers_WriterId",
                table: "Publications",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_Writers_WriterId",
                table: "YoutubeVideos",
                column: "WriterId",
                principalTable: "Writers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
