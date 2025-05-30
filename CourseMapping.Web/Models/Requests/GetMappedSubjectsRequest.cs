using System.Collections;

namespace CourseMapping.Web.Models.Requests;

public class GetMappedSubjectsRequest
{
    public required List<string> CurrentSubjects { get; set; }
}