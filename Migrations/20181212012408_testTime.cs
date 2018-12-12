using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class testTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request");

            migrationBuilder.AlterColumn<int>(
                name: "Current",
                table: "Time",
                nullable: false,
                oldClrType: typeof(DateTime))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentTime",
                table: "Request",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request",
                column: "CurrentTime",
                principalTable: "Time",
                principalColumn: "Current",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Current",
                table: "Time",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CurrentTime",
                table: "Request",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Time_CurrentTime",
                table: "Request",
                column: "CurrentTime",
                principalTable: "Time",
                principalColumn: "Current",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
