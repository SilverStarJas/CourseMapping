using CourseMapping.Domain;
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
        // Subject 
        modelBuilder
            .Entity<Subject>()
            .HasKey(s => s.Code)
            .HasName("subjectCode");

        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Name)
            .HasColumnName("SubjectName");
        
        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Description)
            .HasColumnName("SubjectDescription");
        
        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Level)
            .HasColumnName("SubjectLevel");
        
        // Course
        modelBuilder
            .Entity<Course>()
            .HasKey(c => c.Code)
            .HasName("courseCode");
        
        modelBuilder
            .Entity<Course>()
            .Property(s => s.Name)
            .HasColumnName("CourseName");
        
        modelBuilder
            .Entity<Course>()
            .Property(c => c.Description)
            .HasColumnName("CourseDescription");

        modelBuilder
            .Entity<Course>()
            .HasMany(c => c.Subjects)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseCode);
        
        // University
        modelBuilder
            .Entity<University>()
            .HasKey(u => u.Id)
            .HasName("universityId");
        
        modelBuilder
            .Entity<University>()
            .Property(u => u.Name)
            .HasColumnName("UniversityName");
        
        modelBuilder
            .Entity<University>()
            .Property(u => u.Country)
            .HasColumnName("UniversityCountry");

        modelBuilder
            .Entity<University>()
            .HasMany(u => u.Courses)
            .WithOne(c => c.University)
            .HasForeignKey(c => c.UniversityId);
    }
}