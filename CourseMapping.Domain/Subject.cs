namespace CourseMapping.Domain
{
    public class Subject
    {
        // Properties
        public string Code { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        
        // Foreign key and reference navigation to parent class
        public string? CourseCode { get; set; }
        public Course? Course { get; set; }
        
        // Constructor
        public Subject(string code, string name, string description, int level)
        {
            Code = code;
            Name = name;
            Description = description;
            
            if (level >= 1 && level <= 5)
            {
                Level = level;
            }
            else
            {
                var message = "Subject level must be between 1 and 5.";
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public void UpdateSubject(string? name, string? description, int? level)
        {
            if (name != null)
                Name = name;
            
            if (description != null)
                Description = description;
            
            if (level != null)
                if (level >= 1 && level <= 5) 
                    Level = (int)level;

                else
                {
                    var message = "Subject level must be between 1 and 5.";
                    throw new ArgumentOutOfRangeException(message);
                }
        }

        public List<string> MapSubject(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                throw new Exception("Search keyword cannot be null or empty.");
            
            var mappedSubjects = new List<string>();
            if (Name.Contains(keyword))
                mappedSubjects.Add(Code);
            
            return mappedSubjects;
        }
    }
}
