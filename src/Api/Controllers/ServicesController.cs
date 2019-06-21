using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAll()
        {
            return Ok(new[] { "value1", "value2" });
        }
        
        [HttpGet("{workspace}")]
        public async Task<ActionResult<IEnumerable<string>>> GetWorkspace()
        {
            return Ok(new[] { "value1", "value2" });
        }

        // POST api/values
        [HttpPost("{workspace}/{service}")]
        public async Task Post([FromBody] string value)
        {
        }
    }
}
