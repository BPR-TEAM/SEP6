using Microsoft.EntityFrameworkCore.Migrations;

namespace SEP6.Database.Migrations
{
    public partial class userfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_UserId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowUserId",
                table: "Followers",
                newName: "FollowersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                columns: new[] { "FollowersId", "FollowsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowersId",
                table: "Followers",
                column: "FollowersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowersId",
                table: "Followers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowersId",
                table: "Followers",
                newName: "FollowUserId");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Followers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                columns: new[] { "UserId", "FollowUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_UserId",
                table: "Followers",
                column: "UserId");
        }
    }
}
