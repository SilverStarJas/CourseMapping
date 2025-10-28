using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CourseMapping.Tests.IntegrationTests.Repositories;

public class UniversityRepositoryTests : IAsyncLifetime
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UniversityRepository _universityRepository;

    public UniversityRepositoryTests()
    {
        var webAppFactory = new WebApplicationFactory().WithWebHostBuilder(builder => { });
        var scopeFactory = webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _universityRepository = new UniversityRepository(_dbContext, scope.ServiceProvider.GetRequiredService<HybridCache>());
    }
    
    public async Task InitializeAsync()
    {
        _dbContext.Universities.RemoveRange(_dbContext.Universities);
        await _dbContext.SaveChangesAsync();
    }

    // IDisposable 
    public Task DisposeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task GivenANewUniversity_WhenAddedAndRetrieved_ThenCorrectlyInsertedAndReturned()
    {
        // Arrange
        var newUniversity = new University(Guid.CreateVersion7(), "Test University", "Test Country");

        // Act
        await _universityRepository.AddAsync(newUniversity, CancellationToken.None);
        await _dbContext.SaveChangesAsync();
        var universityCount = (await _universityRepository.GetAllUniversitiesAsync(CancellationToken.None)).Count;
        var university = await _universityRepository.GetUniversityByIdAsync(newUniversity.Id, CancellationToken.None);

        // Assert
        universityCount.Should().Be(1);
        university.Should().NotBeNull();
        university.Name.Should().Be("Test University");
        university.Country.Should().Be("Test Country");
    }
}