using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Persistence;

internal class UniversityRepository : IUniversityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UniversityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public University? GetUniversityById(Guid id)
    {
        return _dbContext.Universities
            .Include(u => u.Courses)
            .ThenInclude(c => c.Subjects)
            .FirstOrDefault(u => u.Id == id);
    }

    public List<University> GetAllUniversities()
    {
        return _dbContext.Universities.ToList();
    }
    
    public void Add(University university)
    {
        _dbContext.Add(university);
    }

    public void DeleteUniversity(University university)
    {
        _dbContext.Remove(university);
    }

    public void DeleteCourse(Course course)
    {
        _dbContext.Remove(course);
    }

    public void DeleteSubject(Subject subject)
    {
        _dbContext.Remove(subject);
    }

    public string GetNextCourseCode()
    {
        return $"C-{Random.Shared.Next(1000)}";
    }

    public string GetNextSubjectCode()
    {
        return $"S-{Random.Shared.Next(2000)}";
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}
