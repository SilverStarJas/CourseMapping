using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Configuration;

public class SubjectTypeConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder
            .Property(s => s.Code)
            .IsRequired();

        builder
            .Property(s => s.Name)
            .IsRequired();
        
        builder
            .Property(s => s.Description)
            .IsRequired();
        
        builder
            .Property(s => s.Level)
            .IsRequired();
    }
}