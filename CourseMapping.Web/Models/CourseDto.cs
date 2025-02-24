namespace CourseMapping.Web.Models
{
    public class CourseDto
    {
        public string Name { get; }
        public string Description { get; set; }
        public string Code { get; }
        public List<SubjectDto> Subjects { get; set; }
    }
}
