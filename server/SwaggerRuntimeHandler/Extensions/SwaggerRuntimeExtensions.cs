using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwaggerRuntimeHandler.BackgroundServices;
using SwaggerRuntimeHandler.Swagger;
using SwaggerRuntimeModels.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SwaggerRuntimeHandler.Extensions
{
    public static class SwaggerRuntimeExtensions
    {
        public static IServiceCollection AddSwaggerRuntimeHandler<TRuntimeUpdater>(this IServiceCollection services, string swaggerControllerPrefix, TimeSpan updateInterval)
        where TRuntimeUpdater : class, ISwaggerRuntimeUpdater
        {
            SwaggerUIRuntimeHandler.ControllerEndpointPrefix = swaggerControllerPrefix;
            SwaggerUIRuntimeHandler.UpdateInterval = updateInterval;

            services.AddSingleton<ISwaggerRuntimeUpdater, TRuntimeUpdater>();
            services.AddSingleton<SwaggerUIRuntimeHandler>();
            services.AddHostedService<SwaggerUpdaterBackgroundService>();
            return services;
        }
        
        public static IApplicationBuilder UseSwaggerUIWithRuntimeHandler(
            this IApplicationBuilder app, string url, string action, Action<SwaggerUIOptions> setupAction)
        {
            app.UseSwaggerUI(c =>
            {
                setupAction(c);
                SwaggerUIRuntimeHandler.UiOptions = c;
                SwaggerUIRuntimeHandler.SetBasicUrlDescriptor(url, action);
            });

            app.UseRouter(r =>
            {
                r.MapGet(SwaggerUIRuntimeHandler.ControllerEndpointPrefix + "/{workspace}/{service}/{version}.json", async (request, response, routeData) =>
                {
                    using (var sourceToken = new CancellationTokenSource(SwaggerUIRuntimeHandler.UpdateInterval/1.8))
                    {
                        var swaggerRuntimeUpdater = (ISwaggerRuntimeUpdater)r.ServiceProvider.GetService(typeof(ISwaggerRuntimeUpdater));
                        var definition =  await swaggerRuntimeUpdater.GetOpenApiDefinition(
                            routeData.Values["workspace"] as string, 
                            routeData.Values["service"] as string, 
                            routeData.Values["version"] as string,
                            sourceToken.Token);
                        
                        await response.WriteAsync(definition.Schema.ToString(), sourceToken.Token);
                    }
                });
            });

            return app;
        }
    }
}