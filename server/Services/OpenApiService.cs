using System.Collections.Generic;
using System.IO;
using Models.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Services
{
    public class OpenApiService
    {
        public static SwaggerUIOptions UiOptions;
        public OpenApiDefinition GetServiceDefinition(string workspace, string service, string version)
        {
            return new OpenApiDefinition
            {
                Schema = JObject.Parse(File.ReadAllText(@"/Users/lucian/temp/swagger.json"))
            };
        }

        public List<OpenApiSummary> GetServices()
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