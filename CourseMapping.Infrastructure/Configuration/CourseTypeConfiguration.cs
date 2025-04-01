using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Configuration;

public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .Property(c => c.Code)
            .IsRequired();
        
        builder
            .Property(c => c.Name)
            .IsRequired();
        
        builder
            .Property(c => c.Description)
            .IsRequired();
        
        builder
            .Property(c => c.Subjects)
            .IsRequired();
    }
}