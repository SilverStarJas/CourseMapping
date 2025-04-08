using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities")]
public class UniversitiesController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public UniversitiesController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet("{universityId}", Name = "GetUniversity")]
    public ActionResult<University> GetUniversity(Guid universityId)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");

        var response = university.MapUniversityToResponse();

        return Ok(response);
    }

    [HttpGet(Name = "GetAllUniversities")]
    public ActionResult<List<UniversityResponse>> GetAllUniversities()
    {
        var universities = _universityRepository.GetAllUniversities();

        var response = universities.Select(u => new UniversityResponse
        {
            Id = u.Id,
            Name = u.Name,
            Country = u.Country
        });
        
        return Ok(response);
    }

    [HttpPost(Name = "AddUniversity")]
    public ActionResult<UniversityResponse> CreateUniversity([FromBody] CreateNewUniversityRequest newUniversityRequest)
    {
        var universityId = Guid.NewGuid();

        var newUniversity = new University(universityId, newUniversityRequest.Name, newUniversityRequest.Country);
        
        _universityRepository.Add(newUniversity);
        _universityRepository.SaveChanges();

        var response = newUniversity.MapUniversityToResponse();
        
        return CreatedAtRoute("GetUniversity", new { universityId = newUniversity.Id }, response);
    }

    [HttpPut("{universityId}", Name = "UpdateUniversity")]
    public ActionResult<UniversityResponse> UpdateUniversity(
        Guid universityId,
        [FromBody] UpdateUniversityRequest updateUniversityRequest)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        university.UpdateUniversity(updateUniversityRequest.Name, updateUniversityRequest.Country);
        
        _universityRepository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{universityId}", Name = "DeleteUniversity")]
    public ActionResult<UniversityResponse> DeleteUniversity(Guid universityId)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        _universityRepository.DeleteUniversity(university);
        _universityRepository.SaveChanges();
        
        return NoContent();
    }
}
