﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.OpenApi;
using Newtonsoft.Json.Linq;
using Services.OpenApi;

namespace Api.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        [HttpGet("{workspace}/{service}/{version}")]
        public async Task<ActionResult<IEnumerable<string>>> Get(
            [FromServices] OpenApiRepository openApiRepository, 
            string workspace,
            string service,
            string version,
            CancellationToken cancellationToken)
        {
            return Ok(await openApiRepository.GetApiDefinition(workspace, service, version, cancellationToken));
        }

        // POST api/values
        [HttpPost("{workspace}/{service}/{version}")]
        public async Task<ActionResult> Post(
            [FromServices] OpenApiRepository openApiRepository, 
            string workspace,
            string service,
            string version,
            [FromBody]JObject  openApiDefinition,
            CancellationToken cancellationToken)
        {
            if (workspace.Contains("/") || service.Contains("/") || version.Contains("/")) return BadRequest("The / Character is not allowed");
            await openApiRepository.SaveApiDefinition(workspace, service, version, new OpenApiDefinition {Schema = openApiDefinition}, cancellationToken);
            return Ok();
        }
    }
}
