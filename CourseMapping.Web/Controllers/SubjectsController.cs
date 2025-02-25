using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics;
using CourseMapping.Domain;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseId}")]
    public class SubjectsController : ControllerBase
    {
        private static List<Subject> subjects = new List<Subject>();

        [HttpPost]
        public ActionResult<Subject> CreateSubject(
            string courseCode,
            string subjectCode,
            string subjectName,
            string subjectDescription,
            int subjectLevel,
            [FromBody] Subject subject)
        {
            if (courseCode == null || subjectCode == null || subjectName == null || subjectDescription == null || subjectLevel == null)
            {
                return BadRequest("Missing information.");
            }
            
            if (subject.Code != subjectCode)
            {
                return BadRequest("Subject codes in route and body do not match.");
            }
            
            subjects.Add(subject);
            return CreatedAtAction(nameof(CreateSubject), new { courseCode, subjectCode = subject.Code }, subject);
        }

        [HttpGet]
        public ActionResult<Subject> GetSubject(string courseId)
        {
            if (subjects == null)
            {
                return NotFound("Subjects not found.");
            }

            return Ok(subjects);
        }
    }
}
