using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prensaestudiantil.Web.Migrations
{
    public partial class ModelsRelationShips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Writers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PublicationCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeVideos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(maxLength: 11, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    WriterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeVideos_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Header = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(maxLength: 250, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageDescription = table.Column<string>(maxLength: 150, nullable: true),
                    Author = table.Column<string>(maxLength: 60, nullable: true),
                    PublicationCategoryId = table.Column<int>(nullable: true),
                    WriterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_PublicationCategories_PublicationCategoryId",
                        column: x => x.PublicationCategoryId,
                        principalTable: "PublicationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publications_Writers_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicationImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PublicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationImages_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicationImages_PublicationId",
                table: "PublicationImages",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_PublicationCategoryId",
                table: "Publications",
                column: "PublicationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_WriterId",
                table: "Publications",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_WriterId",
                table: "YoutubeVideos",
                column: "WriterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicationImages");

            migrationBuilder.DropTable(
                name: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "PublicationCategories");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Writers");
        }
    }
}
