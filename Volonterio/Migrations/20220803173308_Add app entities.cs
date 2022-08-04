using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class Addappentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAppGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Meta = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblAppPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblAppUserFriend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppUserFriend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblAppTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppTag_tblAppGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "tblAppGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblAppPostImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppPostImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppPostImage_tblAppPost_PostId",
                        column: x => x.PostId,
                        principalTable: "tblAppPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblAppPostTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppPostTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppPostTag_tblAppPost_PostId",
                        column: x => x.PostId,
                        principalTable: "tblAppPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblAppFriend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserFriendId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppFriend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblAppFriend_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblAppFriend_tblAppUserFriend_UserFriendId",
                        column: x => x.UserFriendId,
                        principalTable: "tblAppUserFriend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAppFriend_UserFriendId",
                table: "tblAppFriend",
                column: "UserFriendId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppFriend_UserId",
                table: "tblAppFriend",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppGroup_UserId",
                table: "tblAppGroup",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppPostImage_PostId",
                table: "tblAppPostImage",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppPostTag_PostId",
                table: "tblAppPostTag",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppTag_GroupId",
                table: "tblAppTag",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAppFriend");

            migrationBuilder.DropTable(
                name: "tblAppPostImage");

            migrationBuilder.DropTable(
                name: "tblAppPostTag");

            migrationBuilder.DropTable(
                name: "tblAppTag");

            migrationBuilder.DropTable(
                name: "tblAppUserFriend");

            migrationBuilder.DropTable(
                name: "tblAppPost");

            migrationBuilder.DropTable(
                name: "tblAppGroup");
        }
    }
}
