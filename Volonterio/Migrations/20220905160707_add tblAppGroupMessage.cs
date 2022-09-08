using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class addtblAppGroupMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAppGroupMessage",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppGroupMessage", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_tblAppGroupMessage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblAppGroupMessage_tblAppGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "tblAppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAppGroupMessage_UserId",
                table: "tblAppGroupMessage",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAppGroupMessage");
        }
    }
}
