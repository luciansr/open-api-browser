using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SwaggerRuntimeHandler.Swagger;
using SwaggerRuntimeModels.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerRuntimeHandler.Extensions
{
    public static class SwaggerRuntimeExtensions
    {
        public static IServiceCollection AddSwaggerRuntimeHandler<T>(this IServiceCollection services, string swaggerControllerPrefix)
        where T: ISwaggerRuntimeUpdater
        {
            services.AddSingleton<SwaggerUIRuntimeHandler>();
            SwaggerUIRuntimeHandler.ControllerEndpointPrefix = swaggerControllerPrefix;
            return services;
        }
        
        public static IApplicationBuilder UseSwaggerUIWithRuntimeHandler(
            this IApplicationBuilder app, string url, string action, Action<SwaggerUIOptions> setupAction)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url, action);
                setupAction(c);
                SwaggerUIRuntimeHandler.UiOptions = c;
                
                c.SwaggerEndpoint($"/{SwaggerUIRuntimeHandler.ControllerEndpointPrefix}/Workspace1/Service1/v1.json", $"Service1 s");
            });

            app.UseRouter(r =>
            {
                r.MapGet(SwaggerUIRuntimeHandler.ControllerEndpointPrefix + "/{workspace}/{service}/{version}.json", async (request, response, routeData) =>
                {
                    var swaggerRuntimeUpdater = (ISwaggerRuntimeUpdater)r.ServiceProvider.GetService(typeof(ISwaggerRuntimeUpdater));
                    var definition =  await swaggerRuntimeUpdater.GetOpenApiDefinition(
                        routeData.Values["workspace"] as string, 
                        routeData.Values["service"] as string, 
                        routeData.Values["version"] as string);
                    await response.WriteAsync(definition.Schema.ToString());
                });
            });

            return app;
        }
    }
}