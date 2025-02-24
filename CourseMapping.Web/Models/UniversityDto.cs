namespace CourseMapping.Web.Models
{
    public class UniversityDto
    {
        public string Name { get; }
        public string Country { get; }
        public List<CourseDto> Courses { get; set; }
    }
}
