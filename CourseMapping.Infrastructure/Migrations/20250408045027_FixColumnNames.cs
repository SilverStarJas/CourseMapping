using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseCode",
                table: "subject",
                newName: "subject_course_code");

            migrationBuilder.RenameIndex(
                name: "IX_subject_CourseCode",
                table: "subject",
                newName: "IX_subject_subject_course_code");

            migrationBuilder.RenameColumn(
                name: "UniversityId",
                table: "course",
                newName: "course_university_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "course",
                newName: "course_name");

            migrationBuilder.RenameIndex(
                name: "IX_course_UniversityId",
                table: "course",
                newName: "IX_course_course_university_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subject_course_code",
                table: "subject",
                newName: "CourseCode");

            migrationBuilder.RenameIndex(
                name: "IX_subject_subject_course_code",
                table: "subject",
                newName: "IX_subject_CourseCode");

            migrationBuilder.RenameColumn(
                name: "course_university_id",
                table: "course",
                newName: "UniversityId");

            migrationBuilder.RenameColumn(
                name: "course_name",
                table: "course",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_course_course_university_id",
                table: "course",
                newName: "IX_course_UniversityId");
        }
    }
}
