namespace CourseMapping.Web.Models
{
    public class SubjectResponse
    {
        public required string Code { get; init; } 
        public required string Name { get; set; } 
        public required string Description { get; set; }
        public required int Level { get; set; }
    }
}
