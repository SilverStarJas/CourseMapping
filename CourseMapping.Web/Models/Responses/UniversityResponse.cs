namespace CourseMapping.Web.Models.Responses;

public record UniversityResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public required string Country { get; set; }
}
