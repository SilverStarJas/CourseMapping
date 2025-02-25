using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities")]
    public class UniversitiesController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<UniversityDto> GetUniversity(int universityId)
        {
            var university = new UniversityDto { Id = universityId, Name = "Example University", Country = "Example Country" };
            return Ok(university);
        }
        
        [HttpPost]
        public ActionResult<UniversityDto> CreateUniversity(
            int universityId,
            [FromBody] UniversityDto university)
        {
            if (university == null)
            {
                return BadRequest("University data is required.");
            }

            if (university.Id != universityId)
            {
                return BadRequest("University IDs in route and body do not match.");
            }

            if (string.IsNullOrWhiteSpace(university.Name))
            {
                return BadRequest("University name is required.");
            }

            if (string.IsNullOrWhiteSpace(university.Country))
            {
                return BadRequest("University country is required.");
            }

            return CreatedAtAction(nameof(GetUniversity), new { universityId = university.Id }, university);
        }
    }
}