using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    University? GetById(Guid id);

    public List<Course>? GetCourses(Guid universityId);
    
    public List<Subject>? GetSubjects(Guid universityId, string courseCode);

    void Add(University university);

    void Delete(University university);

    string GetNextCourseCode();
    
    string GetNextSubjectCode();
    
    void SaveChanges();
}
