using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using CourseMapping.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses")]
    [OutputCache(PolicyName = "Expire1Minutes")]
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
            
            if (HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) is ApplicationDbContext dbContext)
            {
                var entry = dbContext.Entry(newCourse);
                entry.Property("UniversityId").CurrentValue = universityId;
                await dbContext.Courses.AddAsync(newCourse, cancellationToken);
            }

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
