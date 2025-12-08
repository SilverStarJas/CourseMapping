using CourseMapping.Domain.Exceptions;

namespace CourseMapping.Domain
{
    public class Subject
    {
        // Properties
        public string Code { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        
        // Constructor
        public Subject(string code, string name, string description, int level)
        {
            if (level is < 1 or > 5)
                throw new SubjectLevelException($"Invalid subject level: {level}. Level must be between 1 and 5.");
            Code = code;
            Name = name;
            Description = description;
            Level = level;
        }

        public void UpdateSubject(string? name, string? description, int? level)
        {
            if (name != null)
                Name = name;
            
            if (description != null)
                Description = description;
            
            if (level != null)
            {
                if (level is < 1 or > 5)
                    throw new SubjectLevelException($"Invalid subject level: {level}. Level must be between 1 and 5.");
                Level = (int)level;
            }
        }
    }
}
