using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class addtblAppUserGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAppUserGroup",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppUserGroup", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_tblAppUserGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblAppUserGroup_tblAppGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "tblAppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAppUserGroup_UserId",
                table: "tblAppUserGroup",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAppUserGroup");
        }
    }
}
