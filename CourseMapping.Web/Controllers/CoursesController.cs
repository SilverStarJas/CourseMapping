using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities/{universityId}/courses")]
public class CoursesController : ControllerBase
{
    //private static Dictionary<Guid, List<CourseDto>> _universityCourses = new Dictionary<Guid, List<CourseDto>>();

    private readonly IUniversityRepository _universityRepository;

    public CoursesController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet(Name = "GetCourses")]
    public ActionResult<List<CourseDto>> GetCourses(Guid universityId)
    {
        //     if (!_universityCourses.ContainsKey(universityId))
        //     {
        //         return NotFound("University not found.");
        //     }
        //
        //     var courses = _universityCourses[universityId];
        //
        //     return Ok(courses);

        var university = _universityRepository.GetById(universityId);
        if (university is null)
            return BadRequest();

        var courses = university.Courses;

        var response = courses.Select(c => new CourseDto
        {
            Code = c.Code,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<CourseDto> CreateCourse(
        Guid universityId,
        [FromBody] CourseCreationDto request)
    {
        // if (!_universityCourses.ContainsKey(universityId))
        // {
        //     _universityCourses[universityId] = new List<CourseDto>();
        // }
        //
        // var courses = _universityCourses[universityId];
        // var courseCode = (course.Name[0] + (courses.Count + 1).ToString()).ToUpper();
        //
        // var finalCourse = new CourseDto()
        // {
        //     Code = courseCode,
        //     Name = course.Name,
        //     Description = course.Description
        // };
        //
        // courses.Add(finalCourse);
        // return CreatedAtRoute("GetCourse", new { universityId = universityId, courseCode = finalCourse.Code },
        //     finalCourse);

        var university = _universityRepository.GetById(universityId);
        if (university is null)
            return BadRequest();

        var courseCode = _universityRepository.GetNextCourseId();
        var newCourse = new Course(request.Name, request.Description, courseCode);

        university.AddCourse(newCourse);

        return CreatedAtRoute("GetCourse", new { universityId = universityId, courseCode = newCourse.Code }, newCourse);
    }
}
