using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;

namespace CourseMapping.Infrastructure.Persistence;

public class UniversityRepository : IUniversityRepository
{
    private readonly List<University> _universities = [];
    public University? GetById(Guid id)
    {
        return _universities.FirstOrDefault(u => u.Id == id);
    }

    public void Add(University university)
    {
        _universities.Add(university);
    }

    public void Delete(University university)
    {
        _universities.Remove(university);
    }

    public string GetNextCourseCode()
    {
        return $"C-{Random.Shared.Next(1000)}";
    }

    public string GetNextSubjectCode()
    {
        return $"S-{Random.Shared.Next(2000)}";
    }
}
