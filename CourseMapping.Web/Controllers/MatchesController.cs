using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models.Requests;
using CourseMapping.Web.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities/{universityId}/mappedSubjects")]
public class MatchesController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public MatchesController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public async Task<ActionResult<MatchedSubjectsResponse>> GetMappedSubjectsAsync(
        [FromRoute] Guid universityId,
        [FromBody] GetMatchedSubjectsRequest request,
        CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
        if (university is null)
            return NotFound("University not found.");

        // var allSubjects = university.

        return Ok(new {});
    }
}