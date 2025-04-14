using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    public Task<University?> GetUniversityByIdAsync(Guid id);
    public Task<List<University>> GetAllUniversitiesAsync();
    
    public Task AddAsync(University university);
    public Task DeleteUniversityAsync(University university);
    public Task DeleteCourseAsync(Course course);
    public Task DeleteSubjectAsync(Subject subject);
    public string GetNextCourseCode();
    public string GetNextSubjectCode();
    public Task SaveChangesAsync();
}