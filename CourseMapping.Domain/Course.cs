using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseMapping.Domain
{
    internal class Course
    {
        // Properties
        public string Name { get; } 

        public string Description { get; set; }
        public string Code { get; }
        public List<Unit> Units { get; }

        public Course(string name, string description, string code)
        {
            Name = name;
            Description = description;
            Code = code;
            Units = [];
        }

        public void AddUnit(Unit unit) 
        { 
            Units.Add(unit);
        }

        public List<Unit> GetUnits() 
        {
            return Units;
        }
    }
}
