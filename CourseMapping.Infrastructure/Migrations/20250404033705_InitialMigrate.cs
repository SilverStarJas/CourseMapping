using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "university",
                columns: table => new
                {
                    university_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    university_name = table.Column<string>(type: "TEXT", nullable: false),
                    university_country = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_university", x => x.university_id);
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    course_code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    course_description = table.Column<string>(type: "TEXT", nullable: false),
                    UniversityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.course_code);
                    table.ForeignKey(
                        name: "FK_course_university_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "university",
                        principalColumn: "university_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    subject_code = table.Column<string>(type: "TEXT", nullable: false),
                    subject_name = table.Column<string>(type: "TEXT", nullable: false),
                    subject_description = table.Column<string>(type: "TEXT", nullable: false),
                    subject_level = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.subject_code);
                    table.ForeignKey(
                        name: "FK_subject_course_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "course",
                        principalColumn: "course_code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_UniversityId",
                table: "course",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_subject_CourseCode",
                table: "subject",
                column: "CourseCode");
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
