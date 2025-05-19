namespace CourseMapping.Web.Models.Requests;

public class UpdateSubjectRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Level { get; set; }
}