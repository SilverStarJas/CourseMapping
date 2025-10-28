using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.EntityFrameworkCore;
using CourseMapping.Infrastructure.Extensions;

namespace CourseMapping.Infrastructure.Persistence;

public class UniversityRepository : IUniversityRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHybridCache _cache;

    public UniversityRepository(ApplicationDbContext dbContext, IHybridCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<University?> GetUniversityByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // Checks cache first and returns if result found
        var cacheKey = $"University:{id}";
        var cachedUniversity = await _cache.GetAsync<University>(cacheKey);
        if (cachedUniversity != null)
            return cachedUniversity;
        
        var university = await _dbContext.Universities
            .Include(u => u.Courses)
            .ThenInclude(c => c.Subjects)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (university != null)
            await _cache.SetAsync(cacheKey, university);
        
        return university;
    }

    public async Task<List<University>> GetAllUniversitiesAsync(CancellationToken cancellationToken)
    {
        // Checks cache first and returns if result found, invalidated by Add/Delete operations
        const string cacheKey = "Universities:All";
        var cachedList = await _cache.GetAsync<List<University>>(cacheKey);
        if (cachedList != null)
            return cachedList;
        
        var universities = await _dbContext.Universities.ToListAsync(cancellationToken);
        await _cache.SetAsync(cacheKey, universities);
        
        return universities;
    }

    public async Task AddAsync(University university, CancellationToken cancellationToken)
    {
        await _dbContext.Universities.AddAsync(university, cancellationToken);
        await _cache.RemoveAsync("Universities:All");
    }

    public async Task DeleteUniversityAsync(University university, CancellationToken cancellationToken)
    {
        _dbContext.Universities.Remove(university);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("Universities:All");
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
