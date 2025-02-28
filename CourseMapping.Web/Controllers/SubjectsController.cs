using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics;
using CourseMapping.Domain;
using CourseMapping.Web.Models;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private static Dictionary<string, List<SubjectDto>> _courseSubjects = new Dictionary<string, List<SubjectDto>>();

        [HttpGet(Name = "GetSubjects")]
        public ActionResult<SubjectDto> GetSubjects(Guid universityId, string courseCode)
        {
            if (!_courseSubjects.ContainsKey(courseCode))
            {
                return NotFound("Course not found.");
            }
            
            var subjects = _courseSubjects[courseCode];
            
            return Ok(subjects);
        }

        [HttpPost]
        public ActionResult<SubjectDto> CreateSubject(
            Guid universityId,
            string courseCode,
            [FromBody] SubjectCreationDto subject)
        {
            if (!_courseSubjects.ContainsKey(courseCode))
            {
                _courseSubjects.Add(courseCode, new List<SubjectDto>());
            }
            
            var subjects = _courseSubjects[courseCode];
            var subjectCode = (subject.Name.Substring(0,3) + (subjects.Count + 1).ToString()).ToUpper();

            var finalSubject = new SubjectDto
            {
                Code = subjectCode,
                Name = subject.Name,
                Description = subject.Description,
                Level = subject.Level
            };
            
            subjects.Add(finalSubject);
            return CreatedAtRoute("GetSubjects", new { universityId, courseCode, subjectCode }, finalSubject);
        }
    }
}
