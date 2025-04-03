using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Persistence.Configurations;

public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .ToTable("course")
            .HasKey(c => c.Code);
        
        builder
            .Property(c => c.Code)
            .HasColumnName("course_code")
            .ValueGeneratedNever();
        
        builder
            .Property(c => c.Description)
            .HasColumnName("course_description");

        builder
            .HasMany(c => c.Subjects)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseCode);
    }
}