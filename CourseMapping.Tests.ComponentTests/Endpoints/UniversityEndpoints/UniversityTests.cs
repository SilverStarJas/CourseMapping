using System.Net;
using System.Net.Http.Json;
using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CourseMapping.Tests.ComponentTests.Fixtures;
using CourseMapping.Web.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace CourseMapping.Tests.ComponentTests.Endpoints.UniversityEndpoints;

public class UniversityTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _dbContext;

    public UniversityTests()
    {
        var webAppFactory = new WebApplicationFactory().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services
                    .Where(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>))
                    .ToList();

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
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(Guid.NewGuid().ToString());
    }

    public async Task InitializeAsync()
    {
        _dbContext.Universities.RemoveRange(_dbContext.Universities);
        await _dbContext.SaveChangesAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GivenAValidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnAUniversityCorrectly()
    {
        var university = new University(Guid.CreateVersion7(), "Test University", "Australia");
        _dbContext.Universities.Add(university);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync($"v1/universities/{university.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Test University");
        content.Should().Contain("Australia");
    }

    [Fact]
    public async Task GivenAnInvalidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnNotFound()
    {
        var invalidId = Guid.CreateVersion7();

        var response = await _client.GetAsync($"v1/universities/{invalidId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenUniversitiesExist_WhenGetAllUniversitiesEndpointIsCalled_ReturnAllUniversities()
    {
        var university1 = new University(Guid.CreateVersion7(), "University One", "Country A");
        var university2 = new University(Guid.CreateVersion7(), "University Two", "Country B");
        _dbContext.Universities.AddRange(university1, university2);
        await _dbContext.SaveChangesAsync();

        var response = await _client.GetAsync("v1/universities");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _dbContext.Universities.Should().Contain(university1);
        _dbContext.Universities.Should().Contain(university2);
        _dbContext.Universities.Count().Should().Be(2);
    }

    [Fact]
    public async Task GivenValidInput_WhenUpdateUniversityEndpointIsCalled_ReturnUpdatedUniversity()
    {
        var university = new University(Guid.CreateVersion7(), "Old Name", "Old Country");
        _dbContext.Universities.Add(university);
        await _dbContext.SaveChangesAsync();

        var updateRequest = new UpdateUniversityRequest
        {
            Name = "New Name",
            Country = "New Country"
        };

        var updateResponse = await _client.PutAsJsonAsync($"v1/universities/{university.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var response = await _client.GetAsync($"v1/universities/{university.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUniversity = await response.Content.ReadFromJsonAsync<University>();
        updatedUniversity.Should().NotBeNull();
        updatedUniversity!.Id.Should().Be(university.Id);
        updatedUniversity.Name.Should().Be("New Name");
        updatedUniversity.Country.Should().Be("New Country");
    }

    [Fact]
    public async Task GivenAValidUniversityId_WhenDeleteUniversityEndpointIsCalled_ReturnNoContent()
    {
        var university = new University(Guid.CreateVersion7(), "Test University", "Australia");
        _dbContext.Universities.Add(university);
        await _dbContext.SaveChangesAsync();

        var response = await _client.DeleteAsync($"v1/universities/{university.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var content = _dbContext.Universities.ToList();
        content.Should().BeEmpty();
    }
}
