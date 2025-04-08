using CourseMapping.Domain;
using CourseMapping.Web.Models;

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

    public static CourseResponse MapCourseToResponse(this Course course)
    {
        return new CourseResponse
        {
            Code = course.Code,
            Name = course.Name,
            Description = course.Description
        };
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
}