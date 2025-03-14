using CourseMapping.Domain;
using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities/{universityId}/courses")]
public class CoursesController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public CoursesController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet(Name = "GetCourses")]
    public ActionResult<List<CourseResponse>> GetCourses(Guid universityId)
    {
        var university = _universityRepository.GetById(universityId);
        if (university is null)
            return NotFound("University not found.");

        var courses = university.Courses;

        var response = courses.Select(c => new CourseResponse
        {
            Code = c.Code,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<CourseResponse> CreateCourse(
        Guid universityId,
        [FromBody] CreateNewCourseRequest newCourseRequest)
    {
        var university = _universityRepository.GetById(universityId);
        if (university is null)
            return NotFound("University not found.");

        var courseCode = _universityRepository.GetNextCourseCode();
        var newCourse = new Course(courseCode, newCourseRequest.Name, newCourseRequest.Description);
        
        university.AddCourse(newCourse);
        
        var courseResponse = new CourseResponse
        {
            Code = newCourse.Code,
            Name = newCourse.Name,
            Description = newCourse.Description
        };

        return CreatedAtRoute("GetCourses", new { universityId = universityId, courseCode = newCourse.Code }, newCourse);
    }
}