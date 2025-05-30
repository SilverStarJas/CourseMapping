using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models.Requests;
using CourseMapping.Web.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public CoursesController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        [HttpGet("{courseCode}", Name = "GetCourse")]
        public async Task<ActionResult<CourseResponse>> GetCourseByCodeAsync(Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var response = course.MapCourseToResponse();

            return Ok(response);
        }

        [HttpGet(Name = "GetAllCourses")]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCoursesAsync(Guid universityId, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var courses = university.Courses.ToList();

            var response = courses.MapAllCoursesToResponse();

            return Ok(response);
        }
        
        [HttpGet("{courseCode}/mapped/{subjectKeyword}", Name = "GetMappedSubjects")]
        public async Task<ActionResult<List<string>>> GetMappedSubjectsAsync(
            Guid universityId, string courseCode,
            string subjectKeyword,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            var mappedSubjects = course.Subjects
                .Select(s => s.TryMatchSubject(subjectKeyword))
                .Where(s => s != null)
                .ToList();

            if (mappedSubjects.Count == 0)
                return NotFound("No mapped subjects found.");

            return Ok(mappedSubjects);
        }

        [HttpPost(Name = "AddCourse")]
        public async Task<ActionResult<CourseResponse>> CreateCourseAsync(
            Guid universityId,
            [FromBody] CreateNewCourseRequest newCourseRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var courseCode = _universityRepository.GetNextCourseCode();
            var newCourse = new Course(courseCode, newCourseRequest.Name, newCourseRequest.Description);

            university.AddCourse(newCourse);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            var response = newCourse.MapCourseToResponse();

            return CreatedAtRoute("GetCourse", new { universityId, courseCode = newCourse.Code }, response);
        }

        [HttpPut("{courseCode}", Name = "UpdateCourse")]
        public async Task<IActionResult> UpdateCourseAsync(
            Guid universityId, string courseCode,
            [FromBody] UpdateCourseRequest updateCourseRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return NotFound("Course not found.");

            course.UpdateCourse(updateCourseRequest.Name, updateCourseRequest.Description);

            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{courseCode}", Name = "DeleteCourse")]
        public async Task<IActionResult> DeleteCourseAsync(Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return NotFound("University not found.");

            university.RemoveCourse(courseCode);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
