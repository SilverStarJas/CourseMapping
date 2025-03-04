using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics;
using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public SubjectsController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        
        [HttpGet(Name = "GetSubjects")]
        public ActionResult<SubjectResponse> GetSubjects(string courseCode)
        {
            var course = _courseRepository.GetCourseByCode(courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }
            
            var subjects = course.Subjects;

            var response = subjects.Select(s => new SubjectResponse
            {
                Code = s.Code,
                Name = s.Name,
                Description = s.Description
            });
            
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<SubjectResponse> CreateSubject(
            Guid universityId,
            string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            var course = _courseRepository.GetCourseByCode(courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }
            
            var subjectCode = _courseRepository.GetNextSubjectCode();
            var newSubject = new Subject(subjectCode, newSubjectRequest.Name, newSubjectRequest.Description, newSubjectRequest.Level);
            
            course.AddSubjects(newSubject);
            
            return CreatedAtRoute("GetSubjects", new {courseCode = courseCode, subjectCode = newSubject.Code}, newSubject);
        }
    }
}
