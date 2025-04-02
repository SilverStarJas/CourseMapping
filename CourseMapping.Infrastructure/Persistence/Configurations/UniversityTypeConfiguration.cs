using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Persistence.Configurations;

public class UniversityTypeConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder
            .ToTable("University");
        
        builder
            .Property(u => u.Id)
            .HasColumnName("university_id")
            .ValueGeneratedOnAdd();

        builder
            .Property(u => u.Name)
            .HasColumnName("university_name");
        
        builder
            .Property(u => u.Country)
            .HasColumnName("university_country");

        builder
            .HasMany(u => u.Courses)
            .WithOne(c => c.University)
            .HasForeignKey(c => c.UniversityId);
    }
}