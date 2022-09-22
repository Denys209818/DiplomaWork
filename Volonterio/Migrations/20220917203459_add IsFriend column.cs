using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class addIsFriendcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFriend",
                table: "tblAppUserFriend",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFriend",
                table: "tblAppFriend",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFriend",
                table: "tblAppUserFriend");

            migrationBuilder.DropColumn(
                name: "IsFriend",
                table: "tblAppFriend");
        }
    }
}
