//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using SwaggerRuntimeModels.Swagger;
//
//namespace SwaggerRuntimeHandler.Controllers
//{
////    [Route("api/[controller]")]
//    public class SwaggerRuntimeController : ControllerBase
//    {
//        // GET api/values/5
//        [HttpGet("{workspace}/{service}/{version}.json")]
//        public async Task<ActionResult<string>> Get(string workspace, string service, string version, [FromServices] ISwaggerRuntimeUpdater swaggerRuntimeUpdater)
//        {
//            var openApiDefinition = await swaggerRuntimeUpdater.GetOpenApiDefinition(workspace, service, version);
//            return openApiDefinition.Schema.ToString();
//        }
//    }
//}