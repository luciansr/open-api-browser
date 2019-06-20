using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using SwaggerRuntimeModels.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerRuntimeHandler.Swagger
{
    internal class SwaggerUIRuntimeHandler 
    {
        public static SwaggerUIOptions UiOptions { get; set; }
        public static string ControllerEndpointPrefix { get; set; }
        
        
        private readonly ISwaggerRuntimeUpdater _swaggerRuntimeUpdater;
        private static UrlDescriptor basicUrlDescriptor;

        public SwaggerUIRuntimeHandler(ISwaggerRuntimeUpdater swaggerRuntimeUpdater)
        {
            _swaggerRuntimeUpdater = swaggerRuntimeUpdater;
        }

        public static void SetBasicUrlDescriptor(string url, string name)
        {
            basicUrlDescriptor = new UrlDescriptor() {Url = url, Name = name};
            UiOptions.SwaggerEndpoint(url, name);
        }

        public static void ResetEndpoints()
        {
            UiOptions.ConfigObject.Urls = null;
        }

        public static void SwaggerEndpoint(string url, string name)
        {
            UiOptions.ConfigObject.Urls = (IEnumerable<UrlDescriptor>) new List<UrlDescriptor>(UiOptions.ConfigObject.Urls ?? Enumerable.Empty<UrlDescriptor>())
            {
                new UrlDescriptor() { Url = url, Name = name }
            };
        }
    }
}