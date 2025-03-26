using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    public University? GetUniversityById(Guid id);
    public List<University> GetAllUniversities();

    public List<Course>? GetCourses(Guid universityId);

    public Course? GetCourseByCode(Guid universityId, string courseCode);
    
    public List<Subject>? GetSubjects(Guid universityId, string courseCode);

    public Subject? GetSubjectByCode(Guid universityId, string courseCode, string subjectCode);

    public void Add(University university);

    public void DeleteUniversity(University university);
    
    public void DeleteCourse(Course course);
    
    public void DeleteSubject(Subject subject);
    
    string GetNextCourseCode();
    
    string GetNextSubjectCode();
    
    void SaveChanges();
}
