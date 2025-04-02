using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Universities_UniversityId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Courses_CourseCode",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "Universities",
                newName: "University");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "University",
                newName: "university_id");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Subject",
                newName: "subject_code");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_CourseCode",
                table: "Subject",
                newName: "IX_Subject_CourseCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Course",
                newName: "course_code");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_UniversityId",
                table: "Course",
                newName: "IX_Course_UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_University_UniversityId",
                table: "Course",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "university_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Course_CourseCode",
                table: "Subject",
                column: "CourseCode",
                principalTable: "Course",
                principalColumn: "course_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_University_UniversityId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Course_CourseCode",
                table: "Subject");

            migrationBuilder.RenameTable(
                name: "University",
                newName: "Universities");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameColumn(
                name: "university_id",
                table: "Universities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "subject_code",
                table: "Subjects",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_CourseCode",
                table: "Subjects",
                newName: "IX_Subjects_CourseCode");

            migrationBuilder.RenameColumn(
                name: "course_code",
                table: "Courses",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Course_UniversityId",
                table: "Courses",
                newName: "IX_Courses_UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Universities_UniversityId",
                table: "Courses",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Courses_CourseCode",
                table: "Subjects",
                column: "CourseCode",
                principalTable: "Courses",
                principalColumn: "Code");
        }
    }
}
