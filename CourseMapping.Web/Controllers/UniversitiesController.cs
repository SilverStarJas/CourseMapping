using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Extensions.Controller;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
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
        public async Task<ActionResult<University>> GetUniversityAsync(Guid universityId)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId);
            if (university is null)
                return NotFound("University not found.");

            var response = university.MapUniversityToResponse();

            return Ok(response);
        }

        [HttpGet(Name = "GetAllUniversities")]
        public async Task<ActionResult<List<UniversityResponse>>> GetAllUniversitiesAsync()
        {
            var universities = await _universityRepository.GetAllUniversitiesAsync();

            var response = universities.MapAllUniversitiesToResponse();

            return Ok(response);
        }

        [HttpPost(Name = "AddUniversity")]
        public async Task<ActionResult<UniversityResponse>> CreateUniversityAsync([FromBody] CreateNewUniversityRequest newUniversityRequest)
        {
            var universityId = Guid.NewGuid();

            var newUniversity = new University(universityId, newUniversityRequest.Name, newUniversityRequest.Country);

            await _universityRepository.AddAsync(newUniversity);
            await _universityRepository.SaveChangesAsync();

            var response = newUniversity.MapUniversityToResponse();

            return CreatedAtRoute("GetUniversity", new { universityId = newUniversity.Id }, response);
        }

        [HttpPut("{universityId}", Name = "UpdateUniversity")]
        public async Task<ActionResult<UniversityResponse>> UpdateUniversityAsync(
            Guid universityId,
            [FromBody] UpdateUniversityRequest updateUniversityRequest)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId);
            if (university is null)
                return NotFound("University not found.");

            university.UpdateUniversity(updateUniversityRequest.Name, updateUniversityRequest.Country);

            await _universityRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{universityId}", Name = "DeleteUniversity")]
        public async Task<IActionResult> DeleteUniversityAsync(Guid universityId)
        {
            var university = await _universityRepository.GetUniversityByIdAsync(universityId);
            if (university is null)
                return NotFound("University not found.");

            await _universityRepository.DeleteUniversityAsync(university);
            await _universityRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
