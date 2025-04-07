using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<University> Universities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UniversityTypeConfiguration())
            .ApplyConfiguration(new CourseTypeConfiguration())
            .ApplyConfiguration(new SubjectTypeConfiguration());
    }
}