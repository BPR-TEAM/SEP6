using Microsoft.EntityFrameworkCore.Migrations;

namespace SEP6.Database.Migrations
{
    public partial class usertoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Token",
                table: "Users",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");
        }
    }
}
