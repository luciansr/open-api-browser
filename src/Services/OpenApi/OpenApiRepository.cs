using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.OpenApi;
using Newtonsoft.Json.Linq;

namespace Services.OpenApi
{
    public class OpenApiRepository
    {
        public async Task SaveApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken)
        {
        }

        public async Task<OpenApiDefinition> GetApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken)
        {
            return new OpenApiDefinition
            {
                Schema = JObject.Parse(File.ReadAllText(@"./TempResources/openApiExample.json"))
            };
        }
        
        public async Task<IEnumerable<OpenApiSummary>> GetAllApiDefinitions(CancellationToken cancellationToken)
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
    }
}
