using CourseMapping.Domain;
using Microsoft.EntityFrameworkCore;

namespace CourseMapping.Infrastructure.Persistence.Abstraction;

public interface IUniversityRepository
{
    University? GetById(Guid id);

    void Add(University university);

    void Delete(University university);

    string GetNextCourseCode();
    
    string GetNextSubjectCode();
}
