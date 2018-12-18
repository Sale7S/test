using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class heads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HoD",
                table: "HoD");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "HoD");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "HoD",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoD",
                table: "HoD",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_HoD_User_Username",
                table: "HoD",
                column: "Username",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoD_User_Username",
                table: "HoD");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoD",
                table: "HoD");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "HoD");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "HoD",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoD",
                table: "HoD",
                column: "ID");
        }
    }
}
