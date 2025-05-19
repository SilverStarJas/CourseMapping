using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using CourseMapping.Web.Models.Requests;
using CourseMapping.Web.Models.Responses;
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

        [HttpGet("{subjectCode}", Name = "GetSubject")]
        public async Task<ActionResult<SubjectResponse>> GetSubjectByCodeAsync(
            Guid universityId, string courseCode, string subjectCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
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

        [HttpGet(Name = "GetAllSubjects")]
        public async Task<ActionResult<List<SubjectResponse>>> GetAllSubjectsAsync(
            Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var subjects = course.Subjects;

            var response = subjects.MapAllSubjectsToResponse();

            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult<SubjectResponse>> CreateSubjectAsync(
            Guid universityId, string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var subjectCode = _universityRepository.GetNextSubjectCode();
            var newSubject = new Subject(subjectCode, newSubjectRequest.Name, newSubjectRequest.Description, newSubjectRequest.Level);

            course.AddSubject(newSubject);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            var response = newSubject.MapSubjectToResponse();

            return CreatedAtRoute("GetSubject", new { universityId, courseCode, subjectCode = newSubject.Code }, response);
        }

        [HttpPut("{subjectCode}", Name = "UpdateSubject")]
        public async Task<IActionResult> UpdateSubjectAsync(
            Guid universityId, string courseCode, string subjectCode,
            [FromBody] UpdateSubjectRequest updateSubjectRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return NotFound("Subject not found.");

            subject.UpdateSubject(updateSubjectRequest.Name, updateSubjectRequest.Description, updateSubjectRequest.Level);

            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{subjectCode}", Name = "DeleteSubject")]
        public async Task<IActionResult> DeleteSubjectAsync(
            Guid universityId, string courseCode, string subjectCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");
            
            course.RemoveSubject(subjectCode);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
