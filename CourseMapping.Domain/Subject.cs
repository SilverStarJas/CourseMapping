﻿namespace CourseMapping.Domain
{
    public class Subject
    {
        // Properties
        public string Code { get; init; }
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
    }
}
