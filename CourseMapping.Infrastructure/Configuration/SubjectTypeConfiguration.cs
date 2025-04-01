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
    }
}