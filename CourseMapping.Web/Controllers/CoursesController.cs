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
            if (universityId == null || courseCode == null || courseName == null || courseDescription == null)
            {
                return BadRequest("Missing information.");
            }
            
            if (course.Code != courseCode)
            {
                return BadRequest("Course codes in route and body do not match.");
            }
            
            courses.Add(course);
            return CreatedAtAction(nameof(GetCourse), new { universityId, courseCode = course.Code }, course);
        }
        
        [HttpGet]
        public ActionResult<Course> GetCourse(int universityId)
        {
            if (courses == null)
            {
                return NotFound("Courses not found.");
            }
            
            return Ok(courses);
        }
    }
}
