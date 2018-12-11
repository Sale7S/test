using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class timedRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Section_SectionNumber",
                table: "Request");

            migrationBuilder.AlterColumn<string>(
                name: "SectionNumber",
                table: "Request",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentTime",
                table: "Request",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Time",
                columns: table => new
                {
                    Current = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time", x => x.Current);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_CurrentTime",
                table: "Request",
                column: "CurrentTime");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request",
                column: "CurrentTime",
                principalTable: "Time",
                principalColumn: "Current",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Section_SectionNumber",
                table: "Request",
                column: "SectionNumber",
                principalTable: "Section",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Section_SectionNumber",
                table: "Request");

            migrationBuilder.DropTable(
                name: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Request_CurrentTime",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "CurrentTime",
                table: "Request");

            migrationBuilder.AlterColumn<string>(
                name: "SectionNumber",
                table: "Request",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Section_SectionNumber",
                table: "Request",
                column: "SectionNumber",
                principalTable: "Section",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
