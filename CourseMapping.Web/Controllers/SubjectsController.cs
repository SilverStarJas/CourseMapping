using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public SubjectsController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }
        
        [HttpGet(Name = "GetSubjects")]
        public ActionResult<SubjectResponse> GetSubjects(Guid universityId, string courseCode)
        {
            var university = _universityRepository.GetById(universityId);
            if (university is null)
            {
                return NotFound("University not found.");
            }

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }
            
            var subjects = course.Subjects;

            var response = subjects.Select(s => new SubjectResponse
            {
                Code = s.Code,
                Name = s.Name,
                Description = s.Description,
                Level = s.Level
            });
            
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<SubjectResponse> CreateSubject(
            Guid universityId,
            string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            var university = _universityRepository.GetById(universityId);
            if (university is null)
            {
                return NotFound("University not found.");
            }
            
            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }

            var subjectCode = _universityRepository.GetNextSubjectCode();
            var newSubject = new Subject(subjectCode, newSubjectRequest.Name, newSubjectRequest.Description, newSubjectRequest.Level);
            
            course.AddSubject(newSubject);
            _universityRepository.SaveChanges();
            
            var subjectResponse = new SubjectResponse
            {
                Code = newSubject.Code,
                Name = newSubject.Name,
                Description = newSubject.Description
            };
            
            return CreatedAtRoute("GetSubjects", new {universityId = universityId, courseCode = courseCode, subjectCode = newSubject.Code}, subjectResponse);
        }
    }
}
