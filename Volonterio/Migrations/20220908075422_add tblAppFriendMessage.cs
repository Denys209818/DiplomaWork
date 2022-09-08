using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class addtblAppFriendMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAppFriendMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserFriendId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppFriendMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppFriendMessage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblAppFriendMessage_tblAppUserFriend_UserFriendId",
                        column: x => x.UserFriendId,
                        principalTable: "tblAppUserFriend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAppFriendMessage_UserFriendId",
                table: "tblAppFriendMessage",
                column: "UserFriendId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppFriendMessage_UserId",
                table: "tblAppFriendMessage",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAppFriendMessage");
        }
    }
}
