using CourseMapping.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities")]
    public class UniversitiesController : ControllerBase
    {
        private static List<University> universities = new List<University>();
        
        [HttpPost]
        public ActionResult<University> CreateUniversity(
            int universityId,
            string universityName,
            string universityCountry,
            [FromBody] University university)
        {
            if (universityId == null || universityName == null || universityCountry == null)
            {
                return BadRequest("Missing information.");
            }

            if (university.Id != universityId)
            {
                return BadRequest("University IDs in route and body do not match.");
            }
            
            universities.Add(university);
            return CreatedAtAction(nameof(GetUniversity), new { universityId = university.Id }, university);
        }
        
        [HttpGet("{universityId}")]
        public ActionResult<University> GetUniversity(int universityId)
        {
            var university = universities.FirstOrDefault(u => u.Id == universityId);
            if (university == null)
            {
                return NotFound("University not found.");
            }
            
            return Ok(university);
        }
    }
}