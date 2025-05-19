namespace CourseMapping.Web.Models.Requests;

public class CreateNewUniversityRequest
{
    public required string Name { get; set; }
    public required string Country { get; set; }
}
