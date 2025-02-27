using CourseMapping.Domain;
using CourseMapping.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities")]
    public class UniversitiesController : ControllerBase
    {
        private static List<UniversityDto> _universities = new List<UniversityDto>();
        
        [HttpGet("{universityId}", Name = "GetUniversity")]
        public ActionResult<University> GetUniversity(Guid universityId)
        {
            var university = _universities.FirstOrDefault(u => u.Id == universityId);
            if (university == null)
            {
                return NotFound("University not found.");
            }
            
            return Ok(university);
        }
        
        [HttpPost]
        public ActionResult<UniversityDto> CreateUniversity(
            [FromBody] UniversityCreationDto university)
        {
            // Generate and set an ID for the created University 
            var universityId = Guid.NewGuid();

            var finalUniversity = new UniversityDto()
            {
                Id = universityId,
                Name = university.Name,
                Country = university.Country,
            };
            
            _universities.Add(finalUniversity);
            return CreatedAtRoute("GetUniversity", new { universityId = finalUniversity.Id }, finalUniversity);
        }
    }
}