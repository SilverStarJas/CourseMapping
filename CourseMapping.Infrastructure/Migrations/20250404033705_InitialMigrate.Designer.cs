﻿// <auto-generated />
using System;
using CourseMapping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourseMapping.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250404033705_InitialMigrate")]
    partial class InitialMigrate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("CourseMapping.Domain.Course", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("TEXT")
                        .HasColumnName("course_code");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("course_description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UniversityId")
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.HasIndex("UniversityId");

                    b.ToTable("course", (string)null);
                });

            modelBuilder.Entity("CourseMapping.Domain.Subject", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("TEXT")
                        .HasColumnName("subject_code");

                    b.Property<string>("CourseCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("subject_description");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER")
                        .HasColumnName("subject_level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("subject_name");

                    b.HasKey("Code");

                    b.HasIndex("CourseCode");

                    b.ToTable("subject", (string)null);
                });

            modelBuilder.Entity("CourseMapping.Domain.University", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("university_id");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("university_country");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("university_name");

                    b.HasKey("Id");

                    b.ToTable("university", (string)null);
                });

            modelBuilder.Entity("CourseMapping.Domain.Course", b =>
                {
                    b.HasOne("CourseMapping.Domain.University", "University")
                        .WithMany("Courses")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("CourseMapping.Domain.Subject", b =>
                {
                    b.HasOne("CourseMapping.Domain.Course", "Course")
                        .WithMany("Subjects")
                        .HasForeignKey("CourseCode");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("CourseMapping.Domain.Course", b =>
                {
                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("CourseMapping.Domain.University", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
