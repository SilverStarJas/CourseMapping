﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new ArgumentOutOfRangeException("Subject level must be between 1 and 5.");
            }
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
