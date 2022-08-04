using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class Addconstraintswithpostsandgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "tblAppPost",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblAppPost_GroupId",
                table: "tblAppPost",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblAppPost_tblAppGroup_GroupId",
                table: "tblAppPost",
                column: "GroupId",
                principalTable: "tblAppGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblAppPost_tblAppGroup_GroupId",
                table: "tblAppPost");

            migrationBuilder.DropIndex(
                name: "IX_tblAppPost_GroupId",
                table: "tblAppPost");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "tblAppPost");
        }
    }
}
