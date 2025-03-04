using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseMapping.Domain
{
    public class Course
    {
        // Properties
        public string Code { get; }
        public string Name { get; } 
        public string Description { get; set; }
        public List<Subject> Subjects { get; set; }

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
