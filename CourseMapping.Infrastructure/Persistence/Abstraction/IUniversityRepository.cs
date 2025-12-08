using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    public Task<University?> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<List<University>> GetAllUniversitiesAsync(CancellationToken cancellationToken);
    public Task AddAsync(University university, CancellationToken cancellationToken);
    public Task DeleteUniversityByIdAsync(Guid universityId, CancellationToken cancellationToken);
    public Task DeleteCourseByCodeAsync(string courseCode, CancellationToken cancellationToken);
    public Task DeleteSubjectByCodeAsync(string subjectCode, CancellationToken cancellationToken); 
    public string GetNextCourseCode();
    public string GetNextSubjectCode();
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}