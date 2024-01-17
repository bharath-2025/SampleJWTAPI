using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        [HttpGet("allcourses")]
        public async Task<ActionResult> GetCourses()
        {
            return Ok("All Courses will be fetched Successfully");
        }
    }
}
