using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using CourseMapping.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CourseMapping.Tests.IntegrationTests.Repositories;

public class UniversityRepositoryTests : IAsyncLifetime
{
    
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _dbContext;

    public UniversityRepositoryTests()
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

                var connectionString = GetConnectionString();
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString));
            });
        });

        var scopeFactory = webAppFactory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _client = webAppFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(Guid.NewGuid().ToString());
    }
    
    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("CourseMappingDb");
        
        return connectionString;
    }
    
    public async Task InitializeAsync()
    {
        _dbContext.Universities.RemoveRange(_dbContext.Universities);
        await _dbContext.SaveChangesAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task GivenANewUniversity_WhenANewUniversityIsAdded_ThenUniversityIsInsertedIntoTheDatabase()
    {
        // Arrange
        var newUniversity = new University(Guid.CreateVersion7(), "Test University", "Test Country");
        
        // Act
        await _dbContext.AddAsync(newUniversity, CancellationToken.None);
        await _dbContext.SaveChangesAsync();

        // Assert
        var universityCount = _dbContext.Universities.ToList().Count;
        universityCount.Should().Be(1);
    }
    
    [Fact]
    public async Task GivenAnExistingUniversity_WhenGetUniversityByIdIsCalled_ThenReturnsCorrectUniversity()
    {
        // Arrange
        var existingUniversity = new University(Guid.CreateVersion7(), "Test University", "Test Country");
        await _dbContext.AddAsync(existingUniversity, CancellationToken.None);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var university = await _dbContext.Universities.FindAsync(existingUniversity.Id, CancellationToken.None);

        // Assert
        university.Should().NotBeNull();
        university.Name.Should().Be("Test University");
        university.Country.Should().Be("Test Country");
    }
}