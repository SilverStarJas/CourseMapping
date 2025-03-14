using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Persistence;

public class UniversityRepository : IUniversityRepository
{
    // private readonly List<University> _universities = [];
    private readonly ApplicationDbContext _dbContext;

    public UniversityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public University? GetById(Guid id)
    {
        return _dbContext.Universities.FirstOrDefault(u => u.Id == id);
    }

    public Course? GetCourseByUniversityId(Guid id)
    {
        return _dbContext.Courses.FirstOrDefault(c => c.UniversityId == id);
    }
    
    public void Add(University university)
    {
        _dbContext.Add(university);
    }

    public void Delete(University university)
    {
        _dbContext.Remove(university);
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
