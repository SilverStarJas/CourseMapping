using CourseMapping.Domain;
using CourseMapping.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Services
{
    public class CourseService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHybridCache _cache;
        public CourseService(ApplicationDbContext dbContext, IHybridCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<Course?> GetCourseAsync(string code)
        {
            var cacheKey = $"course_{code}";
            var cachedCourse = await _cache.GetAsync<Course>(cacheKey);
            if (cachedCourse != null)
                return cachedCourse;

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Code == code);
            if (course != null)
                await _cache.SetAsync(cacheKey, course, TimeSpan.FromMinutes(10));
            return course;
        }
    }
}
