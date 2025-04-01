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
            .HasName("subject_code");
        
        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Code)
            .HasColumnName("subject_code");

        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Name)
            .HasColumnName("subject_name");
        
        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Description)
            .HasColumnName("subject_description");
        
        modelBuilder
            .Entity<Subject>()
            .Property(s => s.Level)
            .HasColumnName("subject_level");
        
        // Course
        modelBuilder
            .Entity<Course>()
            .HasKey(c => c.Code)
            .HasName("course_code");
        
        modelBuilder
            .Entity<Course>()
            .Property(c => c.Code)
            .HasColumnName("course_code");
        
        modelBuilder
            .Entity<Course>()
            .Property(s => s.Name)
            .HasColumnName("course_name");
        
        modelBuilder
            .Entity<Course>()
            .Property(c => c.Description)
            .HasColumnName("course_description");

        modelBuilder
            .Entity<Course>()
            .HasMany(c => c.Subjects)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseCode);
        
        // University
        modelBuilder
            .Entity<University>()
            .HasKey(u => u.Id)
            .HasName("university_id");
        
        modelBuilder
            .Entity<University>()
            .Property(u => u.Id)
            .HasColumnName("university_id");
        
        modelBuilder
            .Entity<University>()
            .Property(u => u.Name)
            .HasColumnName("university_name");
        
        modelBuilder
            .Entity<University>()
            .Property(u => u.Country)
            .HasColumnName("university_country");

        modelBuilder
            .Entity<University>()
            .HasMany(u => u.Courses)
            .WithOne(c => c.University)
            .HasForeignKey(c => c.UniversityId);
    }
}