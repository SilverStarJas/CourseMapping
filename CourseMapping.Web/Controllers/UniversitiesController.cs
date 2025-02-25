using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities")]
    public class UniversitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<UniversityDto> GetUniversity(int universityId)
        {
            var university = new UniversityDto();
            return Ok(university);
        }
        
        [HttpPost]
        public ActionResult<UniversityDto> CreateUniversity(
            int universityId,
            string countryName,
            string universityName,
            [FromBody] UniversityDto university)
        {
            if (universityId.ToString() == null)
            {
                return BadRequest("University ID required.");
            } 
            {
                return BadRequest("University must have an ID.");
            }

            if (string.IsNullOrEmpty(countryName))
            {
                return BadRequest("Country name is required.");
            }

            if (string.IsNullOrEmpty(universityName))
            {
                return BadRequest("University name is required.");
            }

            return CreatedAtAction(nameof(GetUniversity), new { universityName = university.Name }, university);
        }
    }
}
