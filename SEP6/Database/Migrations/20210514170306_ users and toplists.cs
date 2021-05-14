using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEP6.Database.Migrations
{
    public partial class usersandtoplists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieToplists",
                columns: table => new
                {
                    MoviesId = table.Column<long>(type: "INTEGER", nullable: false),
                    ToplistsesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieToplists", x => new { x.MoviesId, x.ToplistsesId });
                    table.ForeignKey(
                        name: "FK_MovieToplists_movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "movies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieToplists_TopLists_ToplistsesId",
                        column: x => x.ToplistsesId,
                        principalTable: "TopLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieToplists_ToplistsesId",
                table: "MovieToplists",
                column: "ToplistsesId");

            migrationBuilder.CreateIndex(
                name: "IX_TopLists_UserId",
                table: "TopLists",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieToplists");

            migrationBuilder.DropTable(
                name: "TopLists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
