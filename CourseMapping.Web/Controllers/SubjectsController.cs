using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics;
using CourseMapping.Domain;
using CourseMapping.Infrastructure.Persistence.Abstraction;
using CourseMapping.Web.Models;

namespace CourseMapping.Web.Controllers
{
    [ApiController]
    [Route("v1/universities/{universityId}/courses/{courseCode}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;

        public SubjectsController(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }
        
        [HttpGet(Name = "GetSubjects")]
        public ActionResult<SubjectResponse> GetSubjects(Guid universityId, string courseCode)
        {
            var university = _universityRepository.GetById(universityId);
            if (university is null)
            {
                return NotFound("University not found.");
            }
            
            // var subjects = course.Subjects;
            //
            // var response = subjects.Select(s => new SubjectResponse
            // {
            //     Code = s.Code,
            //     Name = s.Name,
            //     Description = s.Description,
            //     Level = s.Level
            // });
            //
            // return Ok(response);

            return null;
        }

        [HttpPost]
        public ActionResult<SubjectResponse> CreateSubject(
            Guid universityId,
            string courseCode,
            [FromBody] CreateNewSubjectRequest newSubjectRequest)
        {
            return null;
        }
    }
}
