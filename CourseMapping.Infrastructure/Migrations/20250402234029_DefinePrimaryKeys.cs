using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefinePrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "university_id",
                table: "University");

            migrationBuilder.DropPrimaryKey(
                name: "subject_code",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "course_code",
                table: "Course");

            migrationBuilder.AddPrimaryKey(
                name: "pk_university",
                table: "University",
                column: "university_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_subject",
                table: "Subject",
                column: "subject_code");

            migrationBuilder.AddPrimaryKey(
                name: "pk_course",
                table: "Course",
                column: "course_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_university",
                table: "University");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subject",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "pk_course",
                table: "Course");

            migrationBuilder.AddPrimaryKey(
                name: "university_id",
                table: "University",
                column: "university_id");

            migrationBuilder.AddPrimaryKey(
                name: "subject_code",
                table: "Subject",
                column: "subject_code");

            migrationBuilder.AddPrimaryKey(
                name: "course_code",
                table: "Course",
                column: "course_code");
        }
    }
}
