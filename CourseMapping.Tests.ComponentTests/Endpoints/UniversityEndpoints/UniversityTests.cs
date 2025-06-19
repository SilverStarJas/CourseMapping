using System.Net;
using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseMapping.Tests.ComponentTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace CourseMapping.Tests.ComponentTests.Endpoints.UniversityEndpoints;

public class UniversityTests : IClassFixture<WebApplicationFactory<Web.Program>>
{
    private readonly HttpClient _client;
    private readonly TestDbContext _dbContext;

    public UniversityTests(WebApplicationFactory<Web.Program> factory)
    {
        var webAppFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IUniversityRepository, UniversityRepository>();
                
                services.AddDbContext<TestDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
        
        var scopeFactory = webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        _client = webAppFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GivenAValidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnAUniversityCorrectly()
    {
        // Arrange
        var university = new University(Guid.NewGuid(), "Test University", "Australia");
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();

        // Act
        var response = await _client.GetAsync($"v1/universities/{university.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Test University");
        content.Should().Contain("Australia");
    }
}