using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Subject>()
            .HasKey(s => s.Code)
            .HasName("subjectCode");
        
        modelBuilder
            .Entity<Course>()
            .HasKey(c => c.Code)
            .HasName("courseCode");
        
        modelBuilder
            .Entity<University>()
            .HasKey(u => u.Id)
            .HasName("universityId");
    }
}