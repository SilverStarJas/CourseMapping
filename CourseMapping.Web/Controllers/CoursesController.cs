using CourseMapping.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    public class CoursesController : ControllerBase
    {
        private static List<Course> _courses = new List<Course>();
        
        [HttpPost]
        public ActionResult<Course> CreateCourse(
            [FromBody] Course course)
        {
            _courses.Add(course);
            return CreatedAtAction(nameof(GetCourse), new { courseCode = course.Code }, course);
        }
        
        [HttpGet]
        public ActionResult<Course> GetCourse(int universityId)
        {
            if (_courses == null)
            {
                return NotFound("Courses not found.");
            }
            
            return Ok(_courses);
        }
    }
}
