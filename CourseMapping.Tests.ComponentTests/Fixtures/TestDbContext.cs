using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Tests.ComponentTests.Fixtures;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

    public DbSet<University> Universities { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UniversityTypeConfiguration())
            .ApplyConfiguration(new CourseTypeConfiguration())
            .ApplyConfiguration(new SubjectTypeConfiguration());
    }
}