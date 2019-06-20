using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Models.OpenApi;
using SwaggerRuntimeModels.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerRuntimeHandler.Swagger
{
    public class SwaggerUIRuntimeHandler 
    {
        internal static SwaggerUIOptions UiOptions { get; set; }
        internal static string ControllerEndpointPrefix { get; set; }
        internal static TimeSpan UpdateInterval { get; set; }
        
        
        private readonly ISwaggerRuntimeUpdater _swaggerRuntimeUpdater;
        private static (string Url, string Name) basicUrlDescriptor;

        private string GetUrl(OpenApiSummary summary) => $"/{ControllerEndpointPrefix}/{summary.Workspace}/{summary.ServiceName}/{summary.Version}.json";

        public SwaggerUIRuntimeHandler(ISwaggerRuntimeUpdater swaggerRuntimeUpdater)
        {
            _swaggerRuntimeUpdater = swaggerRuntimeUpdater;
        }

        internal static void SetBasicUrlDescriptor(string url, string name)
        {
            basicUrlDescriptor = (url, name);
            UiOptions.SwaggerEndpoint(url, name);
        }

        private static void ResetEndpoints()
        {
            UiOptions.ConfigObject.Urls = null;
            UiOptions.SwaggerEndpoint(basicUrlDescriptor.Url, basicUrlDescriptor.Name);
        }

        private static void SwaggerEndpoint(string url, string name)
        {
            UiOptions.SwaggerEndpoint(url, name);
        }

        internal async Task UpdateEndpoints(CancellationToken cancellationToken)
        {
            var updatedOpenApiList = await _swaggerRuntimeUpdater.GetUpdatedOpenApiList(cancellationToken);
            ResetEndpoints();

            foreach (var openApiSummary in updatedOpenApiList)
            {
                SwaggerEndpoint(GetUrl(openApiSummary),$"{openApiSummary.ServiceName} - {openApiSummary.Version}");
            }
        }
    }
}