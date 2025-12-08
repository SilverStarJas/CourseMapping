using CourseMapping.Domain;
using CourseMapping.Domain.Exceptions;
using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    [OutputCache(PolicyName = "Expire1Minutes")]
    public class SubjectsController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public SubjectsController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        [HttpGet("{subjectCode}", Name = "GetSubject")]
        public async Task<IActionResult> GetSubjectByCodeAsync(
            Guid universityId, string courseCode, string subjectCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                throw new UniversityNotFoundException($"University with ID '{universityId}' not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                throw new CourseNotFoundException($"Course with code '{courseCode}' not found in university '{universityId}'.");

            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                throw new SubjectNotFoundException($"Subject with code '{subjectCode}' not found in course '{courseCode}'.");

            var response = subject.MapSubjectToResponse();

            return Ok(response);
        }

        [HttpGet(Name = "GetAllSubjects")]
        public async Task<IActionResult> GetAllSubjectsAsync(
            Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return ValidationProblem(statusCode: 404);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return ValidationProblem(statusCode: 404);

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
                return ValidationProblem(statusCode: 404);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return ValidationProblem(statusCode: 404);
            
            var subjectCode = _universityRepository.GetNextSubjectCode();
            var newSubject = new Subject(subjectCode, newSubjectRequest.Name, newSubjectRequest.Description, newSubjectRequest.Level);

            if (HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) is ApplicationDbContext dbContext)
            {
                var entry = dbContext.Entry(newSubject);
                entry.Property("CourseCode").CurrentValue = courseCode;
                await dbContext.Subjects.AddAsync(newSubject, cancellationToken);
            }

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
                return ValidationProblem(statusCode: 404);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return ValidationProblem(statusCode: 404);

            var subject = course.Subjects.FirstOrDefault(s => s.Code == subjectCode);
            if (subject is null)
                return ValidationProblem(statusCode: 404);

            subject.UpdateSubject(updateSubjectRequest.Name, updateSubjectRequest.Description, updateSubjectRequest.Level);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{subjectCode}", Name = "DeleteSubject")]
        public async Task<IActionResult> DeleteSubjectAsync(
            Guid universityId, string courseCode, string subjectCode, CancellationToken cancellationToken)
        {
            try
            {
                await _universityRepository.DeleteSubjectByCodeAsync(subjectCode, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return ValidationProblem(statusCode: 404);
            }
        }
    }
}
