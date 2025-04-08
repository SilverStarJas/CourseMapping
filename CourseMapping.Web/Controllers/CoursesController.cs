using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
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

    [HttpGet("{courseCode}", Name = "GetCourse")]
    public ActionResult<CourseResponse> GetCourseByCode(Guid universityId, string courseCode)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
        if (course is null)
            return NotFound("Course not found.");

        var response = course.MapCourseToResponse();
        
        return Ok(response);
    }
    
    [HttpGet(Name = "GetAllCourses")]
    public ActionResult<List<CourseResponse>> GetAllCourses(Guid universityId)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");

        var courses = university.Courses.ToList();
        
        var response = courses.Select(c => new CourseResponse
        {
            Code = c.Code,
            Name = c.Name,
            Description = c.Description
        });

        return Ok(response);
    }

    [HttpPost(Name = "AddCourse")]
    public ActionResult<CourseResponse> CreateCourse(
        Guid universityId,
        [FromBody] CreateNewCourseRequest newCourseRequest)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");

        var courseCode = _universityRepository.GetNextCourseCode();
        var newCourse = new Course(courseCode, newCourseRequest.Name, newCourseRequest.Description);
        
        university.AddCourse(newCourse);
        _universityRepository.SaveChanges();

        var response = newCourse.MapCourseToResponse();
        
        return CreatedAtRoute("GetCourse", new { universityId, courseCode = newCourse.Code }, response);
    }

    [HttpPut("{courseCode}", Name = "UpdateCourse")]
    public ActionResult<CourseResponse> UpdateCourse(
        Guid universityId, string courseCode,
        [FromBody] UpdateCourseRequest updateCourseRequest)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
        
        if (course is null)
            return NotFound("Course not found.");

        course.UpdateCourse(updateCourseRequest.Name, updateCourseRequest.Description);
        
        _universityRepository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{courseCode}", Name = "DeleteCourse")]
    public ActionResult<CourseResponse> DeleteCourse(
        Guid universityId, string courseCode)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        var course = university.Courses.FirstOrDefault(c => c.Code == courseCode);
        if (course is null)
            return NotFound("Course not found.");
        
        _universityRepository.DeleteCourse(course);
        _universityRepository.SaveChanges();
        
        return NoContent();
    }
}