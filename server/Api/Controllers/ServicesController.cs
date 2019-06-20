using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Services;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        
        [HttpPost("add")]
        public async Task<ActionResult<IEnumerable<string>>> AddService()
        {
            var guid = Guid.NewGuid().ToString();
            OpenApiService.UiOptions.SwaggerEndpoint($"/api/services/Workspace1/{guid}/v1.json", $"Service1 {guid}");
            return Ok(new[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{workspace}/{service}/{version}.json")]
        public async Task<ActionResult<string>> Get(string workspace, string service, string version, [FromServices] OpenApiService openApiService)
        {
            return Ok(openApiService.GetServiceDefinition(workspace, service, version).Schema);
        }

        // POST api/values
        [HttpPost("{workspace}/{service}")]
        public async Task Post([FromBody] string value)
        {
        }
    }
}
