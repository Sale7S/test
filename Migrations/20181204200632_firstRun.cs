using Microsoft.EntityFrameworkCore.Migrations;

namespace COCAS.Migrations
{
    public partial class firstRun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentCode",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Student_StudentID",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserType_Type",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Instructor_DepartmentCode",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Instructor");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "User",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Section",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "Section",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Schedule",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Course",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentCode",
                table: "Course",
                column: "DepartmentCode",
                principalTable: "Department",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Student_StudentID",
                table: "Schedule",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserType_Type",
                table: "User",
                column: "Type",
                principalTable: "UserType",
                principalColumn: "Type",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentCode",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Student_StudentID",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserType_Type",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Section",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "Section",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Schedule",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Instructor",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Course",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_DepartmentCode",
                table: "Instructor",
                column: "DepartmentCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentCode",
                table: "Course",
                column: "DepartmentCode",
                principalTable: "Department",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Department_DepartmentCode",
                table: "Instructor",
                column: "DepartmentCode",
                principalTable: "Department",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Student_StudentID",
                table: "Schedule",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserType_Type",
                table: "User",
                column: "Type",
                principalTable: "UserType",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
