using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NotFoundObjectResult = CourseMapping.Web.Extensions.Results.NotFoundObjectResult;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    [OutputCache(PolicyName = "Expire5Minutes")]
    public class SubjectsController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public SubjectsController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        private IActionResult UniversityNotFound(Guid universityId)
        {
            return new NotFoundObjectResult($"University with ID {universityId} not found.", HttpContext.Request.Path);
        }
        private IActionResult CourseNotFound(string courseCode)
        {
            return new NotFoundObjectResult($"Course with code '{courseCode}' not found.", HttpContext.Request.Path);
        }
        private IActionResult SubjectNotFound(string subjectCode)
        {
            return new NotFoundObjectResult($"Subject with code '{subjectCode}' not found.", HttpContext.Request.Path);
        }

        [HttpGet("{subjectCode}", Name = "GetSubject")]
        public async Task<IActionResult> GetSubjectByCodeAsync(
            Guid universityId, string courseCode, string subjectCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return SubjectNotFound(subjectCode);

            var response = subject.MapSubjectToResponse();
            return Ok(response);
        }

        [HttpGet(Name = "GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjectsAsync(
            Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            var subjects = course.Subjects;
            var response = subjects.MapAllSubjectsToResponse();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubjectAsync(
            Guid universityId, string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

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
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return SubjectNotFound(subjectCode);

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
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            course.RemoveSubject(subjectCode);
            await _universityRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
