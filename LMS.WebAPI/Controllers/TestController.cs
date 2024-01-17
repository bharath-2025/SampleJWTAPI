using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CustomPolicy1")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Method()
        {
            return "hello World";
        }
    }
}
