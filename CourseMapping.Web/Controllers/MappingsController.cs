using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models.Requests;
using CourseMapping.Web.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers;

[ApiController]
[Route("v1/universities/{universityId}/mappedSubjects")]
public class MappingsController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public MappingsController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public async Task<ActionResult<MappedSubjectsResponse>> GetMappedSubjectsAsync(
        [FromRoute] Guid universityId,
        [FromBody] GetMappedSubjectsRequest request,
        CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
        if (university is null)
            return NotFound("University not found.");

        // This should all be in Domain
        List<Subject> allSubjects = new List<Subject>();
        
        var currentSubjects = request.CurrentSubjects;
        
        foreach (var name in currentSubjects)
        {
            if (allSubjects.Any(x => x.Name == name))
                continue;
        
            // for ()
            //     return NoContent(); // temporary placeholder
        }

        return Ok(); // temporary placeholder
    }
}