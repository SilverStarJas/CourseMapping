namespace CourseMapping.Web.Models.Requests;

public class CreateNewCourseRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}