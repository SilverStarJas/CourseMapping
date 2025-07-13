using System.Net;
using System.Net.Http.Json;
using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseMapping.Tests.ComponentTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace CourseMapping.Tests.ComponentTests.Endpoints.UniversityEndpoints;

public class UniversityTests
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _dbContext;

    public UniversityTests()
    {
        var webAppFactory = new WebApplicationFactory().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.Where(
                    d => d.ServiceType == 
                         typeof(IDbContextOptionsConfiguration<ApplicationDbContext>)).ToList();

                foreach (var descriptor in dbContextDescriptor)
                    services.Remove(descriptor);
                
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
        
        var scopeFactory = webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _client = webAppFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GivenAValidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnAUniversityCorrectly()
    {
        // Arrange
        var university = new University(Guid.CreateVersion7(), "Test University", "Australia");
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
        var invalidId = Guid.CreateVersion7();

        // Act
        var response = await _client.GetAsync($"v1/universities/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GivenUniversitiesExist_WhenGetAllUniversitiesEndpointIsCalled_ReturnAllUniversities()
    {
        // Arrange
        var university1 = new University(Guid.CreateVersion7(), "University One", "Country A");
        var university2 = new University(Guid.CreateVersion7(), "University Two", "Country B");
        _dbContext.Universities.AddRange(university1, university2);
        _dbContext.SaveChanges();

        // Act
        var response = await _client.GetAsync("v1/universities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<List<University>>();
        content.Should().Contain(university1);
        content.Should().Contain(university2);
        content.Count.Should().Be(2);
    }

    [Fact]
    public async Task GivenValidInput_WhenUpdateUniversityEndpointIsCalled_ReturnUpdatedUniversity()
    {
        // Arrange
        var university = new University(Guid.CreateVersion7(), "Old Name", "Old Country");
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();

        var updateRequest = new { Name = "New Name", Country = "New Country" };

        // Act
        var response = await _client.PutAsJsonAsync($"v1/universities/{university.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        university.Name.Should().Be("New Name");
        university.Country.Should().Be("New Country");
    }
    
    [Fact]
    public async Task GivenAValidUniversityId_WhenDeleteUniversityEndpointIsCalled_ReturnNoContent()
    {
        // Arrange
        var university = new University(Guid.CreateVersion7(), "Test University", "Australia");
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();

        // Act
        var response = await _client.DeleteAsync($"v1/universities/{university.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var content = await response.Content.ReadFromJsonAsync<List<University>>();
        content.Should().BeEmpty();
    }
}