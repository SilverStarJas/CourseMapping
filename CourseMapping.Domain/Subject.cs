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
            Level = level;
        }

        public void UpdateSubject(string? name, string? description, int? level)
        {
            if (name != null)
                Name = name;
            
            if (description != null)
                Description = description;
            
            if (level is >= 1 and <= 5)
            {
                Level = (int)level;
            }
        }
    }
}
