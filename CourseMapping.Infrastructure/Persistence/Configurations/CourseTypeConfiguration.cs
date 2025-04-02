using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Persistence.Configurations;

public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .ToTable("Course");

        builder
            .HasKey(c => c.Code)
            .HasName("pk_course");
        
        builder
            .Property(c => c.Code)
            .HasColumnName("course_code")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(c => c.Name)
            .HasColumnName("course_name");
        
        builder
            .Property(c => c.Description)
            .HasColumnName("course_description");

        builder
            .HasMany(c => c.Subjects)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseCode);
    }
}