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

    public async Task<University?> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Universities
            .Include(u => u.Courses)
            .ThenInclude(c => c.Subjects)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<List<University>> GetAllUniversitiesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Universities.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(University university, CancellationToken cancellationToken)
    {
        await _dbContext.Universities.AddAsync(university, cancellationToken);
    }

    public async Task DeleteUniversityAsync(University university, CancellationToken cancellationToken)
    {
        _dbContext.Universities.Remove(university);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCourseAsync(Course course, CancellationToken cancellationToken)
    {
        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubjectAsync(Subject subject, CancellationToken cancellationToken)
    {
        _dbContext.Subjects.Remove(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public string GetNextCourseCode()
    {
        return $"C-{Random.Shared.Next(1000)}";
    }

    public string GetNextSubjectCode()
    {
        return $"S-{Random.Shared.Next(2000)}";
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
