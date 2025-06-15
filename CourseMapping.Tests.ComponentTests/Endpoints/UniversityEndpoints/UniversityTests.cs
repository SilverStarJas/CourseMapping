using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseMapping.Infrastructure;
using Xunit;

namespace CourseMapping.Tests.ComponentTests.Endpoints.UniversityEndpoints;

public class UniversityTests : IClassFixture<WebApplicationFactory<Web.Program>>
{
    private readonly WebApplicationFactory<Web.Program> _factory;

    public UniversityTests(WebApplicationFactory<Web.Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });
    }

    [Fact]
    public async Task GivenAValidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnAUniversityCorrectly()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
    }
}