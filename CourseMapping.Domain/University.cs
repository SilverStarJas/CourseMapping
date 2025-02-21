using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseMapping.Domain
{
    internal class University
    {
        // Properties
        public string Name { get; }
        public string Country { get; }
        public List<Course> Courses { get; }

        // Constructor
        public University(string name, string country)
        {
            Name = name;
            Country = country;
            Courses = [];
        }

        public void AddCourse(Course newCourse)
        {
            Courses.Add(newCourse);
        }

        public List<Course> GetCourses() 
        {
            return Courses;
        }
    }
}
