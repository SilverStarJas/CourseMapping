using CourseMapping.Domain;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    public University? GetUniversityById(Guid id);
    public List<University> GetAllUniversities();
    
    public void Add(University university);

    public void DeleteUniversity(University university);
    
    public void DeleteCourse(Course course);
    
    public void DeleteSubject(Subject subject);
    
    string GetNextCourseCode();
    
    string GetNextSubjectCode();
    
    void SaveChanges();
}
