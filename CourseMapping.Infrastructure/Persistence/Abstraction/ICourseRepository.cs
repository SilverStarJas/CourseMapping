using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface ICourseRepository
{
    Course? GetCourseByCode(string code);
    
    void Add(Course course);
    
    void Delete(Course course);
    
    string GetNextSubjectCode();
}