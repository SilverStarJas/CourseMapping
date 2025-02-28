using CourseMapping.Domain;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    public class CoursesController : ControllerBase
    {
        private static Dictionary<Guid, List<CourseDto>> _universityCourses = new Dictionary<Guid, List<CourseDto>>();

        [HttpGet(Name = "GetCourse")]
        public ActionResult<CourseDto> GetCourse(Guid universityId)
        {
            if (!_universityCourses.ContainsKey(universityId))
            {
                return NotFound("University not found.");
            }

            var courses = _universityCourses[universityId];

            return Ok(courses);
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourse(
            Guid universityId, 
            [FromBody] CourseCreationDto course)
        {
            if (!_universityCourses.ContainsKey(universityId))
            {
                _universityCourses[universityId] = new List<CourseDto>();
            }

            var courses = _universityCourses[universityId];
            var courseCode = (course.Name[0] + (courses.Count + 1).ToString()).ToUpper();

            var finalCourse = new CourseDto()
            {
                Code = courseCode,
                Name = course.Name,
                Description = course.Description
            };

            courses.Add(finalCourse);
            return CreatedAtRoute("GetCourse", new { universityId = universityId, courseCode = finalCourse.Code },
                finalCourse);
        }
    }
}