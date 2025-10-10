using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CourseMapping.Infrastructure.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CourseMappingDb")));

        services.AddDistributedMemoryCache();
        services.AddMemoryCache();
        services.AddSingleton<IHybridCache, HybridCacheService>();
    }
}