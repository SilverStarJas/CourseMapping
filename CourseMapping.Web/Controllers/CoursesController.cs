using CourseMapping.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    public class CoursesController : ControllerBase
    {
        private static List<Course> courses = new List<Course>();
        
        [HttpPost]
        public ActionResult<Course> CreateCourse(
            int universityId,
            string courseCode,
            string courseName,
            string courseDescription,
            [FromBody] Course course)
        {
            if (universityId == null)
            {
                return BadRequest("University ID is required.");
            }
            
            if (course.Code != courseCode)
            {
                return BadRequest("Course codes in route and body do not match.");
            }
            
            if (string.IsNullOrWhiteSpace(course.Name))
            {
                return BadRequest("Course name is required.");
            }

            if (string.IsNullOrWhiteSpace(course.Description))
            {
                return BadRequest("Course description is required.");
            }
            
            courses.Add(course);
            return CreatedAtAction(nameof(GetCourse), new { universityId, courseCode = course.Code }, course);
        }
        
        [HttpGet("{courseCode}")]
        public ActionResult<Course> GetCourse(int universityId, string courseCode)
        {
            var course = courses.FirstOrDefault(c => c.Code == courseCode);

            if (course == null)
            {
                return NotFound("Course not found.");
            }
            
            return Ok(courses);
        }
    }
}
