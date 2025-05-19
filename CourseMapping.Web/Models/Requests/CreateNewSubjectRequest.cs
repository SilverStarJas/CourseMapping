namespace CourseMapping.Web.Models.Requests;

public class CreateNewSubjectRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Level { get; set; }
}