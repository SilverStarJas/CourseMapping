using System.Net;
using System.Text;
using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseMapping.Tests.ComponentTests.Fixtures;
using FluentAssertions;
using Newtonsoft.Json;
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
    
    [Fact]
    public async Task GivenAnInvalidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"v1/universities/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GivenUniversitiesExist_WhenGetAllUniversitiesEndpointIsCalled_ReturnAllUniversities()
    {
        // Arrange
        var university1 = new University(Guid.NewGuid(), "University One", "Country A");
        var university2 = new University(Guid.NewGuid(), "University Two", "Country B");
        _dbContext.Universities.AddRange(university1, university2);
        _dbContext.SaveChanges();

        // Act
        var response = await _client.GetAsync("v1/universities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("University One");
        content.Should().Contain("University Two");
        content.Length.Should().Be(2);
    }

    [Fact]
    public async Task GivenValidInput_WhenUpdateUniversityEndpointIsCalled_ReturnUpdatedUniversity()
    {
        // Arrange
        var university = new University(Guid.NewGuid(), "Old Name", "Old Country");
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();

        var updateRequest = new { Name = "New Name", Country = "New Country" };
        var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"v1/universities/{university.Id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        university.Name.Should().Be("New Name");
        university.Country.Should().Be("New Country");
    }
    
    [Fact]
    public async Task GivenAValidUniversityId_WhenDeleteUniversityEndpointIsCalled_ReturnNoContent()
    {
        // Arrange
        var university = new University(Guid.NewGuid(), "Test University", "Australia");
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();

        // Act
        var response = await _client.DeleteAsync($"v1/universities/{university.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        university.Should().BeNull();
    }
}