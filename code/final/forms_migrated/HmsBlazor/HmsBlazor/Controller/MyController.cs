using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HmsBlazor.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles="Role.Owner")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string[]> Get()
        {
            string[] values = new string[] { "value1", "value2", "value3" };
            return Ok(values);
        }
    }
}