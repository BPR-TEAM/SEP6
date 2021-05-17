using Microsoft.EntityFrameworkCore.Migrations;

namespace SEP6.Database.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_FollowUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FollowUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowUserId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    FollowUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => new { x.UserId, x.FollowUserId });
                    table.ForeignKey(
                        name: "FK_Followers_Users_FollowsId",
                        column: x => x.FollowsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FollowsId",
                table: "Followers",
                column: "FollowsId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_UserId",
                table: "Followers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.AddColumn<int>(
                name: "FollowUserId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FollowUserId",
                table: "Users",
                column: "FollowUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_FollowUserId",
                table: "Users",
                column: "FollowUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
