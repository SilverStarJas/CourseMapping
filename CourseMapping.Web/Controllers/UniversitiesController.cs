﻿using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
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

        var response = new UniversityResponse
        {
            Id = university.Id,
            Name = university.Name,
            Country = university.Country
        };

        return Ok(response);
    }

    [HttpGet(Name = "GetAllUniversities")]
    public ActionResult<List<UniversityResponse>> GetAllUniversities()
    {
        var universities = _universityRepository.GetAllUniversities();
        return Ok(universities);
    }

    [HttpPost(Name = "AddUniversity")]
    public ActionResult<UniversityResponse> CreateUniversity([FromBody] CreateNewUniversityRequest newUniversityRequest)
    {
        var universityId = Guid.NewGuid();

        var newUniversity = new University(universityId, newUniversityRequest.Name, newUniversityRequest.Country);
        
        _universityRepository.Add(newUniversity);
        _universityRepository.SaveChanges();
        
        return CreatedAtRoute("GetUniversity", new { universityId = newUniversity.Id }, newUniversity);
    }

    [HttpPut("{universityId}", Name = "UpdateUniversity")]
    public ActionResult<UniversityResponse> UpdateUniversity(
        Guid universityId,
        [FromBody] CreateNewUniversityRequest newUniversityRequest)
    {
        var university = _universityRepository.GetUniversityById(universityId);
        if (university is null)
            return NotFound("University not found.");
        
        university.Name = newUniversityRequest.Name;
        university.Country = newUniversityRequest.Country;
        
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
