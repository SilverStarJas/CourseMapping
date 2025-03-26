using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseMapping.Domain
{
    public class University
    {
        // Properties
        public Guid Id { get; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<Course> Courses { get; }

        // Constructor
        public University(Guid id, string name, string country)
        {
            Id = id;
            Name = name;
            Country = country;
            Courses = [];
        }

        public void AddCourse(Course newCourse)
        {
            Courses.Add(newCourse);
        }
    }
}
