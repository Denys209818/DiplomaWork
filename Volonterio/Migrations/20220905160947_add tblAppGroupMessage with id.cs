using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Volonterio.Migrations
{
    public partial class addtblAppGroupMessagewithid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAppGroupMessage",
                table: "tblAppGroupMessage");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "tblAppGroupMessage",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAppGroupMessage",
                table: "tblAppGroupMessage",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblAppGroupMessage_GroupId",
                table: "tblAppGroupMessage",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblAppGroupMessage",
                table: "tblAppGroupMessage");

            migrationBuilder.DropIndex(
                name: "IX_tblAppGroupMessage_GroupId",
                table: "tblAppGroupMessage");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "tblAppGroupMessage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblAppGroupMessage",
                table: "tblAppGroupMessage",
                columns: new[] { "GroupId", "UserId" });
        }
    }
}
