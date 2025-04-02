using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure;

public class ApplicationDbContext : DbContext
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
        
        modelBuilder.Entity<University>()
            .HasKey(u => u.Id)
            .HasName("pk_university");
        
        modelBuilder.Entity<Course>()
            .HasKey(c => c.Code)
            .HasName("pk_course");
        
        modelBuilder.Entity<Subject>()
            .HasKey(s => s.Code)
            .HasName("pk_subject");
    }
}