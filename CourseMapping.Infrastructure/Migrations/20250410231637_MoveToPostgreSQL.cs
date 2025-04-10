using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveToPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "university",
                columns: table => new
                {
                    university_id = table.Column<Guid>(type: "uuid", nullable: false),
                    university_name = table.Column<string>(type: "text", nullable: false),
                    university_country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_university", x => x.university_id);
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    course_code = table.Column<string>(type: "text", nullable: false),
                    course_name = table.Column<string>(type: "text", nullable: false),
                    course_description = table.Column<string>(type: "text", nullable: false),
                    course_university_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.course_code);
                    table.ForeignKey(
                        name: "fk_course_university_id",
                        column: x => x.course_university_id,
                        principalTable: "university",
                        principalColumn: "university_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    subject_code = table.Column<string>(type: "text", nullable: false),
                    subject_name = table.Column<string>(type: "text", nullable: false),
                    subject_description = table.Column<string>(type: "text", nullable: false),
                    subject_level = table.Column<int>(type: "integer", nullable: false),
                    subject_course_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.subject_code);
                    table.ForeignKey(
                        name: "fk_subject_course_code",
                        column: x => x.subject_course_code,
                        principalTable: "course",
                        principalColumn: "course_code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_course_university_id",
                table: "course",
                column: "course_university_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_subject_course_code",
                table: "subject",
                column: "subject_course_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "university");
        }
    }
}
