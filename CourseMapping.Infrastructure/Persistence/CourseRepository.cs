using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence;

public class CourseRepository
{
    private readonly List<Course> _courses = [];

    public Course? GetCourseByCode(string code)
    {
        return _courses.FirstOrDefault(c => c.Code == code);
    }

    public void Add(Course course)
    {
        _courses.Add(course);
    }

    public string GetNextSubjectCode()
    {
        return $"U-{Random.Shared.Next(2000)}";
    }
}