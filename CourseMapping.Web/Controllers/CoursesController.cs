using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using CourseMapping.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NotFoundObjectResult = CourseMapping.Web.Extensions.Results.NotFoundObjectResult;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    [OutputCache(PolicyName = "Expire5Minutes")]
    public class CoursesController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public CoursesController(IUniversityRepository universityRepository)
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

        [HttpGet("{courseCode}", Name = "GetCourse")]
        public async Task<IActionResult> GetCourseByCodeAsync(Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            var response = course.MapCourseToResponse();
            return Ok(response);
        }

        [HttpGet(Name = "GetAllCourses")]
        public async Task<IActionResult> GetAllCoursesAsync(Guid universityId, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var courses = university.Courses.ToList();
            var response = courses.MapAllCoursesToResponse();
            return Ok(response);
        }

        [HttpPost(Name = "AddCourse")]
        public async Task<IActionResult> CreateCourseAsync(
            Guid universityId,
            [FromBody] CreateNewCourseRequest newCourseRequest,
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

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
                return UniversityNotFound(universityId);

            var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
            if (course is null)
                return CourseNotFound(courseCode);

            course.UpdateCourse(updateCourseRequest.Name, updateCourseRequest.Description);

            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{courseCode}", Name = "DeleteCourse")]
        public async Task<IActionResult> DeleteCourseAsync(Guid universityId, string courseCode, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            university.RemoveCourse(courseCode);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
