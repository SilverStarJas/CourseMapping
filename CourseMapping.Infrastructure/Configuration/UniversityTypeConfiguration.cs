using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Configuration;

public class UniversityTypeConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder
            .Property(u => u.Id)
            .IsRequired();
    }
}