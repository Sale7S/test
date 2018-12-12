using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class upload_files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Instructor",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Department",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor",
                column: "DepartmentCode",
                principalTable: "Department",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Department");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Instructor",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor",
                column: "DepartmentCode",
                principalTable: "Department",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
