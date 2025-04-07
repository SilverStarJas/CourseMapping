using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using System.Linq;
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

    public List<Course>? GetCourses(Guid universityId)
    {
        return _dbContext.Courses.Where(c => c.UniversityId == universityId).ToList();
    }

    public Course? GetCourseByCode(Guid universityId, string courseCode)
    {
        return _dbContext.Courses.FirstOrDefault(c => c.UniversityId == universityId && c.Code == courseCode);
    }

    public List<Subject>? GetSubjects(Guid universityId, string courseCode)
    {
        var course = _dbContext.Courses
            .Include(c => c.Subjects)
            .FirstOrDefault(c => c.UniversityId == universityId && c.Code == courseCode);
        
        return course.Subjects.ToList();
    }

    public Subject? GetSubjectByCode(Guid universityId, string courseCode, string subjectCode)
    {
        return _dbContext.Subjects
            .Include(s => s.Course)
            .ThenInclude(c => c.University)
            .FirstOrDefault(s =>
                s.Code == subjectCode &&
                s.Course.Code == courseCode &&
                s.Course.University.Id == universityId);
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
