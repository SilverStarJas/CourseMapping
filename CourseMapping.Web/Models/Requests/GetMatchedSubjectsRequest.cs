using System.Collections;

namespace CourseMapping.Web.Models.Requests;

public class GetMatchedSubjectsRequest
{
    public required List<string> CurrentSubjects { get; set; }
}