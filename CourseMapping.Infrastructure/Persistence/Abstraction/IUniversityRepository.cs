using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    public Task<University?> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<University>> GetAllUniversitiesAsync(CancellationToken cancellationToken);
    public Task AddAsync(University university, CancellationToken cancellationToken);
    public Task DeleteUniversityAsync(University university, CancellationToken cancellationToken);
    public string GetNextCourseCode();
    public string GetNextSubjectCode();
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task RemoveUniversityCacheAsync(Guid universityId, CancellationToken cancellationToken);
}