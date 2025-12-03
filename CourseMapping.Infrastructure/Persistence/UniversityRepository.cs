using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace CourseMapping.Infrastructure.Persistence;

public class UniversityRepository : IUniversityRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly HybridCache _cache;

    public UniversityRepository(ApplicationDbContext dbContext, HybridCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<University?> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // var cacheKey = $"University:{id}";
        var university =
            await _dbContext.Universities
                .Include(u => u.Courses)
                .ThenInclude(c => c.Subjects)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return university;
    }

    public async Task<List<University>> GetAllUniversitiesAsync(CancellationToken cancellationToken)
    {
        const string cacheKey = "Universities:All";
        var cachedList = await _cache.GetOrCreateAsync(
            cacheKey,
            _dbContext,
            async (dbContext, token) => await dbContext.Universities
                .Include(u => u.Courses)
                .ThenInclude(c => c.Subjects)
                .ToListAsync(token),
            cancellationToken : cancellationToken
            );
        return cachedList;
    }

    public async Task AddAsync(University university, CancellationToken cancellationToken)
    {
        await _dbContext.Universities.AddAsync(university, cancellationToken);
        await _cache.RemoveAsync("Universities:All", cancellationToken);
    }

    public async Task DeleteUniversityByIdAsync(Guid universityId, CancellationToken cancellationToken)
    {
        var university = await _dbContext.Universities.FindAsync(new object[] { universityId }, cancellationToken);
        if (university == null) throw new KeyNotFoundException();
        _dbContext.Universities.Remove(university);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCourseByCodeAsync(string courseCode, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Code == courseCode, cancellationToken);
        if (course == null) throw new KeyNotFoundException();
        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubjectByCodeAsync(string subjectCode, CancellationToken cancellationToken)
    {
        var subject = await _dbContext.Subjects.FirstOrDefaultAsync(s => s.Code == subjectCode, cancellationToken);
        if (subject == null) throw new KeyNotFoundException();
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

    public async Task RemoveUniversityCacheAsync(Guid universityId, CancellationToken cancellationToken)
    {
        var cacheKey = $"University:{universityId}";
        await _cache.RemoveAsync(cacheKey, cancellationToken);
    }

    public async Task RemoveAllUniversitiesCacheAsync(CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync("Universities:All", cancellationToken);
    }
}
