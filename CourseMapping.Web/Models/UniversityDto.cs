namespace CourseMapping.Web.Models
{
    public class UniversityDto
    {
        public int Id { get; }
        public string Name { get; }
        public string Country { get; }
        public List<CourseDto> Courses { get; set; }
    }
}
