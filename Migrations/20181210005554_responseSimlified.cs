using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class responseSimlified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.DropIndex(
                name: "IX_Response_RequestID",
                table: "Response");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Response");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentTime",
                table: "Request",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response",
                table: "Response",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request",
                column: "CurrentTime",
                principalTable: "Time",
                principalColumn: "Current",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Response",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentTime",
                table: "Request",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response",
                table: "Response",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Response_RequestID",
                table: "Response",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request",
                column: "CurrentTime",
                principalTable: "Time",
                principalColumn: "Current",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
