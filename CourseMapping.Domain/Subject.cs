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
            Code = code;
            Name = name;
            Description = description;
            
            if (level is >= 1 and <= 5)
            {
                Level = level;
            }
            else
            {
                throw new SubjectLevelException("Subject level must be between 1 and 5.");
            }
        }

        public void UpdateSubject(string? name, string? description, int? level)
        {
            if (name != null)
                Name = name;
            
            if (description != null)
                Description = description;

            switch (level)
            {
                case null:
                    return;
                case >= 1 and <= 5:
                    Level = (int)level;
                    break;
                default:
                    throw new SubjectLevelException("Subject level must be between 1 and 5.");
            }
        }
    }
}
