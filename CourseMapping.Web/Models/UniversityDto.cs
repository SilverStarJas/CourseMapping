using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Models
{
    public class UniversityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}