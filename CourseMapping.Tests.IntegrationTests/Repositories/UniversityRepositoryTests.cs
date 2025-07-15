using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence;
using CourseMapping.Tests.IntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace CourseMapping.Tests.IntegrationTests.Repositories;

public class UniversityRepositoryTests(WebApplicationFactory factory) : Fixtures.IntegrationTests(factory)
{
    [Fact]
    public async Task GivenANewUniversity_WhenANewUniversityIsAdded_ThenUniversityIsInsertedIntoTheDatabase()
    {
        // Arrange
        var newUniversity = new University(Guid.CreateVersion7(), "Test University", "Test Country");
        UniversityRepository universityRepository = new UniversityRepository(DbContext);
        
        
        // Act
        await universityRepository.AddAsync(newUniversity, CancellationToken.None);

        // Assert
        var universityCount = universityRepository.GetAllUniversitiesAsync(CancellationToken.None);
        universityCount.Should().Be(1);
    }
    
    [Fact]
    public async Task GivenAnExistingUniversity_WhenGetUniversityByIdIsCalled_ThenReturnsCorrectUniversity()
    {
        // Arrange
        var existingUniversity = new University(Guid.CreateVersion7(), "Test University", "Test Country");
        await UniversityRepository.AddAsync(existingUniversity, CancellationToken.None);
        
        // Act
        var university = await UniversityRepository.GetUniversityByIdAsync(existingUniversity.Id, CancellationToken.None);

        // Assert
        university.Should().NotBeNull();
        university.Name.Should().Be("Test University");
        university.Country.Should().Be("Test Country");
    }
}