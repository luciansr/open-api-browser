using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwaggerRuntimeModels.Swagger;

namespace Services
{
    public class OpenApiService : ISwaggerRuntimeUpdater
    {
        public async Task<IEnumerable<OpenApiSummary>> GetUpdatedOpenApiList(CancellationToken cancellationToken)
        {
            return new List<OpenApiSummary>
            {
                new OpenApiSummary
                {
                    Version = "v1",
                    ServiceName = "TestService",
                    Workspace = "MyWorkspace"
                },
                new OpenApiSummary
                {
                    Version = "v1",
                    ServiceName = "TestService 2",
                    Workspace = "MyWorkspace"
                }
            };
        }

        public async Task<OpenApiDefinition> GetOpenApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken)
        {
            return new OpenApiDefinition
            {
                Schema = JObject.Parse(File.ReadAllText(@"/Users/lucian/temp/swagger.json"))
            };
        }
    }
}