using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CourseMapping.Tests.IntegrationTests.Repositories;

public class UniversityRepositoryTests : IClassFixture<WebApplicationFactory<Web.Program>>
{
    [Fact]
    public async Task GivenAValidUniversityId_WhenGetUniversityEndpointIsCalled_ReturnAUniversityCorrectly()
    {
        
    }
}