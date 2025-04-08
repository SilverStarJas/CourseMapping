using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFkConstraintName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_course_university_UniversityId",
                table: "course");

            migrationBuilder.DropForeignKey(
                name: "FK_subject_course_CourseCode",
                table: "subject");

            migrationBuilder.AddForeignKey(
                name: "FK_course_university_id",
                table: "course",
                column: "UniversityId",
                principalTable: "university",
                principalColumn: "university_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subject_course_code",
                table: "subject",
                column: "CourseCode",
                principalTable: "course",
                principalColumn: "course_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_course_university_id",
                table: "course");

            migrationBuilder.DropForeignKey(
                name: "FK_subject_course_code",
                table: "subject");

            migrationBuilder.AddForeignKey(
                name: "FK_course_university_UniversityId",
                table: "course",
                column: "UniversityId",
                principalTable: "university",
                principalColumn: "university_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subject_course_CourseCode",
                table: "subject",
                column: "CourseCode",
                principalTable: "course",
                principalColumn: "course_code");
        }
    }
}
