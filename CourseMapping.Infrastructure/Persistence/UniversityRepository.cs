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

    public async Task<University?> GetUniversityByIdAsync(Guid id)
    {
        return await _dbContext.Universities
            .Include(u => u.Courses)
            .ThenInclude(c => c.Subjects)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<University>> GetAllUniversitiesAsync()
    {
        return await _dbContext.Universities.ToListAsync();
    }

    public async Task AddAsync(University university)
    {
        await _dbContext.Universities.AddAsync(university);
    }

    public async Task DeleteUniversityAsync(University university)
    {
        _dbContext.Universities.Remove(university);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(Course course)
    {
        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteSubjectAsync(Subject subject)
    {
        _dbContext.Subjects.Remove(subject);
        await _dbContext.SaveChangesAsync();
    }

    public string GetNextCourseCode()
    {
        return $"C-{Random.Shared.Next(1000)}";
    }

    public string GetNextSubjectCode()
    {
        return $"S-{Random.Shared.Next(2000)}";
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}