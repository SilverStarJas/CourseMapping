using CourseMapping.Domain;
using CourseMapping.Web.Models;
using CourseMapping.Web.Models.Responses;

namespace CourseMapping.Web.Extensions.Controller;

public static class MapDomainModelToResponse
{
    public static UniversityResponse MapUniversityToResponse(this University university)
    {
        return new UniversityResponse
        {
            Id = university.Id,
            Name = university.Name,
            Country = university.Country
        };
    }

    public static List<UniversityResponse> MapAllUniversitiesToResponse(this IEnumerable<University> universities)
    {
        return universities.Select(MapUniversityToResponse).ToList();
    }

    public static CourseResponse MapCourseToResponse(this Course course)
    {
        return new CourseResponse
        {
            Code = course.Code,
            Name = course.Name,
            Description = course.Description
        };
    }

    public static List<CourseResponse> MapAllCoursesToResponse(this IEnumerable<Course> courses)
    {
        return courses.Select(MapCourseToResponse).ToList();
    }

    public static SubjectResponse MapSubjectToResponse(this Subject subject)
    {
        return new SubjectResponse
        {
            Code = subject.Code,
            Name = subject.Name,
            Description = subject.Description,
            Level = subject.Level
        };
    }

    public static List<SubjectResponse> MapAllSubjectsToResponse(this IEnumerable<Subject> subjects)
    {
        return subjects.Select(MapSubjectToResponse).ToList();
    }
}