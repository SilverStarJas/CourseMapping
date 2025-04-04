using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CourseMapping.Infrastructure.Extensions;

public static class AddInfrastructureService
{
    public static void AddInfrastructureServices(IServiceCollection services)
    {
        services.AddScoped<IUniversityRepository, UniversityRepository>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=CourseMapping.db"));
    }
}