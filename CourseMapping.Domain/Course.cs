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
        public string Name { get;  set; } 

        public string Description { get; set; }
        public string Code { get; set; }
        public List<Unit> Units { get; set; }
    }
}
