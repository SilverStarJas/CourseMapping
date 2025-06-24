using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence.Abstraction;

namespace CourseMapping.Tests.IntegrationTests.Fixtures;

public abstract class IntegrationTests(WebApplicationFactory factory)
{
    internal ApplicationDbContext DbContext { get; }
    internal IUniversityRepository UniversityRepository { get; set; }
}