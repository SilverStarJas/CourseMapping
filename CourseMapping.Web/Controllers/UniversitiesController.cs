﻿using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities")]
public class UniversitiesController : ControllerBase
{
    //private static List<UniversityDto> _universities = new List<UniversityDto>();

    private readonly IUniversityRepository _universityRepository;

    public UniversitiesController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet("{universityId}", Name = "GetUniversity")]
    public ActionResult<University> GetUniversity(Guid universityId)
    {
        // var university = _universities.FirstOrDefault(u => u.Id == universityId);
        // if (university == null)
        // {
        //     return NotFound("University not found.");
        // }
        //
        // return Ok(university);

        var university = _universityRepository.GetById(universityId);
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

    [HttpPost]
    public ActionResult<UniversityResponse> CreateUniversity([FromBody] CreateNewUniversityRequest newUniversityRequest)
    {
        // Generate and set an ID for the created University
        var universityId = Guid.NewGuid();

        // var finalUniversity = new UniversityDto()
        // {
        //     Id = universityId,
        //     Name = university.Name,
        //     Country = university.Country,
        // };
        //
        // _universities.Add(finalUniversity);
        // return CreatedAtRoute("GetUniversity", new { universityId = finalUniversity.Id }, finalUniversity);

        var newUniversity = new University(universityId, newUniversityRequest.Name, newUniversityRequest.Country);

        _universityRepository.Add(newUniversity);
        return CreatedAtRoute("GetUniversity", new { universityId = newUniversity.Id }, newUniversity);
    }
}
