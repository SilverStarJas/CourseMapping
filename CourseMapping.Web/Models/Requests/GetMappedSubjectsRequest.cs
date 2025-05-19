namespace CourseMapping.Web.Models.Requests;

public class GetMappedSubjectsRequest
{
    public required List<string> Names { get; set; }
}