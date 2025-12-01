using CourseMapping.Domain.Exceptions;

namespace CourseMapping.Domain
{
    public class Course
    {
        // Properties
        public string Code { get; init; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        
        // Constructor
        public Course(string code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
            Subjects = [];
        }

        public void UpdateCourse(string? name, string? description)
        {
            if (name != null)
                Name = name;
            
            if (description != null)
                Description = description;
        }

        public void AddSubject(Subject subject) 
        {
            if (Subjects.Count < 3)
                Subjects.Add(subject);
            else
                throw new CourseException("Up to three subjects allowed.");
        }

        public void RemoveSubject(string subjectCode)
        {
            var subject = Subjects.FirstOrDefault(s => s.Code == subjectCode);
            
            if (subject is null)
                throw new SubjectException($"Subject {subjectCode} not found in the course.");
            
            Subjects.Remove(subject);
        }
    }
}
