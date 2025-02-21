using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseMapping.Domain
{
    public class Unit
    {
        // Properties
        public string Name { get; }
        public string Description { get; set; }
        public string Code { get; }
        public int Level { get; }

        public Unit(string name, string code, string description, int level)
        {
            Name = name;
            Code = code;
            Description = description;
            Level = level;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
