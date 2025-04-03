using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseMapping.Infrastructure.Persistence.Configurations;

public class SubjectTypeConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder
            .ToTable("subject")
            .HasKey(s => s.Code);

        builder
            .Property(s => s.Code)
            .HasColumnName("subject_code")
            .ValueGeneratedNever();
        
        builder
            .Property(s => s.Name)
            .HasColumnName("subject_name");
        
        builder
            .Property(s => s.Description)
            .HasColumnName("subject_description");
        
        builder
            .Property(s => s.Level)
            .HasColumnName("subject_level");
    }
}