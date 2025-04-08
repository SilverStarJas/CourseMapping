namespace CourseMapping.Domain
{
    public class Course
    {
        // Properties
        public string Code { get; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public List<Subject> Subjects { get; set; }
        
        // Foreign key and reference navigation to parent class
        public Guid UniversityId { get; set; }
        public University? University { get; set; }

        // Constructor
        public Course(string code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
            Subjects = [];
        }

        public void AddSubject(Subject unit) 
        {
            if (Subjects.Count < 3)
                Subjects.Add(unit);
            else
                throw new InvalidOperationException("Up to three subjects allowed.");
        }
    }
}
