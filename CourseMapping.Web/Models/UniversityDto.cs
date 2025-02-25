using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Models
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}