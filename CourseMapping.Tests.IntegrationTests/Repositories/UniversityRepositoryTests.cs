using CourseMapping.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace CourseMapping.Tests.IntegrationTests.Repositories;

public class UniversityRepositoryTests : Fixtures.IntegrationTests
{
    public UniversityRepositoryTests(WebApplicationFactory factory) : base(factory) { }
    
    [Fact]
    public async Task GivenANewUniversity_WhenANewUniversityIsAdded_ThenUniversityIsInsertedIntoTheDatabase()
    {
        // Arrange
        var newUniversity = Guid.CreateVersion7();
        // Add newUniversity to the database using the UniversityRepository
        
        // Act


        // Assert

    }
}