using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
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

        [HttpGet("{subjectCode}", Name = "GetSubjectByCode")]
        public ActionResult<SubjectResponse> GetSubjectByCode(Guid universityId, string courseCode,
            string subjectCode)
        {
            var university = _universityRepository.GetUniversityById(universityId);
            if (university is null)
                return NotFound("University not found.");
            
            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");
            
            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return NotFound("Subject not found.");

            var response = subject.MapSubjectToResponse();
            
            return Ok(response);
        }
        
        [HttpGet(Name = "GetSubjects")]
        public ActionResult<SubjectResponse> GetAllSubjects(Guid universityId, string courseCode)
        {
            var university = _universityRepository.GetUniversityById(universityId);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");
            
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
            Guid universityId, string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            var university = _universityRepository.GetUniversityById(universityId);
            if (university is null)
                return NotFound("University not found.");
            
            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var subjectCode = _universityRepository.GetNextSubjectCode();
            var newSubject = new Subject(subjectCode, newSubjectRequest.Name, newSubjectRequest.Description, newSubjectRequest.Level);
            
            course.AddSubject(newSubject);
            _universityRepository.SaveChanges();
            
            var response = newSubject.MapSubjectToResponse();
            
            return CreatedAtRoute("GetSubjects", new {universityId, courseCode, subjectCode = newSubject.Code}, response);
        }
        
        [HttpPut("{subjectCode}", Name = "UpdateSubject")]
        public ActionResult<SubjectResponse> UpdateSubject(
            Guid universityId, string courseCode, string subjectCode,
            [FromBody] UpdateSubjectRequest updateSubjectRequest)
        {
            var university = _universityRepository.GetUniversityById(universityId);
            if (university is null)
                return NotFound("University not found.");
        
            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");
            
            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return NotFound("Subject not found.");
                
            subject.UpdateSubject(updateSubjectRequest.Name, updateSubjectRequest.Description, updateSubjectRequest.Level);
            
            _universityRepository.SaveChanges();
            
            return NoContent();
        }

        [HttpDelete("{subjectCode}", Name = "DeleteSubject")]
        public ActionResult<SubjectResponse> DeleteSubject(
            Guid universityId, string courseCode, string subjectCode)
        {
            var university = _universityRepository.GetUniversityById(universityId);
            if (university is null)
                return NotFound("University not found.");
        
            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");
            
            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return NotFound("Subject not found.");
            
            _universityRepository.DeleteSubject(subject);
            _universityRepository.SaveChanges();
            
            return NoContent();
        }
    }
}
