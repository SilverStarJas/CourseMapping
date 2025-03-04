namespace CourseMapping.Web.Models;

public record UniversityResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Country { get; init; }
}
