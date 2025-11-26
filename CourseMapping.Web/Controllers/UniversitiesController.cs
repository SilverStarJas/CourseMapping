using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NotFoundObjectResult = CourseMapping.Web.Extensions.Results.NotFoundObjectResult;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities")]
    [OutputCache(PolicyName = "Expire5Minutes")]
    public class UniversitiesController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversitiesController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        private IActionResult UniversityNotFound(Guid universityId)
        {
            return new NotFoundObjectResult($"University with ID {universityId} not found.", HttpContext.Request.Path);
        }

        [HttpGet("{universityId}", Name = "GetUniversity")]
        public async Task<IActionResult> GetUniversityAsync(Guid universityId, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            var response = university.MapUniversityToResponse();
            return Ok(response);
        }

        [HttpGet(Name = "GetAllUniversities")]
        public async Task<ActionResult<List<UniversityResponse>>> GetAllUniversitiesAsync(CancellationToken cancellationToken)
        {
            var universities = await _universityRepository.GetAllUniversitiesAsync(cancellationToken);

            var response = universities.MapAllUniversitiesToResponse();

            return Ok(response);
        }

        [HttpPost(Name = "AddUniversity")]
        public async Task<ActionResult<UniversityResponse>> CreateUniversityAsync(
            [FromBody] CreateNewUniversityRequest newUniversityRequest, 
            CancellationToken cancellationToken)
        {
            var universityId = Guid.CreateVersion7();

            var newUniversity = new University(universityId, newUniversityRequest.Name, newUniversityRequest.Country);

            await _universityRepository.AddAsync(newUniversity, cancellationToken);
            await _universityRepository.SaveChangesAsync(cancellationToken);

            var response = newUniversity.MapUniversityToResponse();

            return CreatedAtRoute("GetUniversity", new { universityId = newUniversity.Id }, response);
        }

        [HttpPut("{universityId}", Name = "UpdateUniversity")]
        public async Task<IActionResult> UpdateUniversityAsync(
            Guid universityId, 
            [FromBody] UpdateUniversityRequest updateUniversityRequest, 
            CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            university.UpdateUniversity(updateUniversityRequest.Name, updateUniversityRequest.Country);
            await _universityRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }

        [HttpDelete("{universityId}", Name = "DeleteUniversity")]
        public async Task<IActionResult> DeleteUniversityAsync(Guid universityId, CancellationToken cancellationToken)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId, cancellationToken);
            if (university is null)
                return UniversityNotFound(universityId);

            await _universityRepository.DeleteUniversityAsync(university, cancellationToken);
            await _universityRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
