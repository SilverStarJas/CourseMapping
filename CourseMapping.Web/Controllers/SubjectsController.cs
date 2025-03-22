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

            var course = _universityRepository.GetCourseByCode(universityId, courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }
            
            var subjects = _universityRepository.GetSubjects(universityId, courseCode);

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
            Guid universityId, string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            var university = _universityRepository.GetById(universityId);
            if (university is null)
            {
                return NotFound("University not found.");
            }
            
            var course = _universityRepository.GetCourseByCode(universityId, courseCode);
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
                Description = newSubject.Description,
                Level = newSubject.Level
            };
            
            return CreatedAtRoute("GetSubjects", new {universityId = universityId, courseCode = courseCode, subjectCode = newSubject.Code}, subjectResponse);
        }
        
        // TODO: Fix PUT and DELETE 

        [HttpPut("{subjectCode}", Name = "UpdateSubject")]
        public ActionResult<SubjectResponse> UpdateSubject(
            Guid universityId, string courseCode, string subjectCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            var university = _universityRepository.GetById(universityId);
            if (university is null)
            {
                return NotFound("University not found.");
            }
            
            var course = _universityRepository.GetCourseByCode(universityId, courseCode);
            if (course is null)
            {
                return NotFound("Course not found.");
            }
            
            var subject = _universityRepository.GetSubjectByCode(universityId, courseCode, subjectCode);
            
            subject.Name = newSubjectRequest.Name;
            subject.Description = newSubjectRequest.Description;
            subject.Level = newSubjectRequest.Level;
            
            _universityRepository.SaveChanges();
            
            return NoContent();
        }

        [HttpDelete("{subjectCode}", Name = "DeleteSubject")]
        public ActionResult<SubjectResponse> DeleteSubject(
            Guid universityId, string courseCode, string subjectCode)
        {
            var subject = _universityRepository.GetSubjectByCode(universityId, courseCode, subjectCode);
            if (subject is null)
                return NotFound("Subject not found.");
            
            _universityRepository.DeleteSubject(subject);
            _universityRepository.SaveChanges();
            
            return NoContent();
        }
    }
}
