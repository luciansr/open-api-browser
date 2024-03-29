using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Services.OpenApi;
using SwaggerRuntimeHandler.Extensions;

namespace Api.Swagger
{
    public static class SwaggerExtensions
    {
        private const string AppName = "Open Api Browser";
        private const string Version = "v1";
        
        public static IServiceCollection AddCustomSwaggerGen(
            this IServiceCollection services, string swaggerPrefix)
        {
            if (services == null)
                throw new ArgumentNullException(nameof (services));
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerExcludeFilter>();
                c.SwaggerDoc($"{Version}", new OpenApiInfo { 
                    Title = $"{AppName}", 
                    Version = $"{Version}", 
                    Description = $"{AppName}",
                });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<GlobalParameters>();
            });
            
            services.AddSwaggerRuntimeHandler<OpenApiUpdater>(swaggerPrefix, TimeSpan.FromMinutes(1));

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(
            this IApplicationBuilder app, string swaggerPrefix)
        {
            if (app == null)
                throw new ArgumentNullException(nameof (app));
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"{swaggerPrefix}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, request) =>
                {
                    var protocol = request.IsHttps ? "https" : "http";
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer()
                        {
                            Description = "My Server",
                            Url = $"{protocol}://{request.Host}",
                        }
                    };
                });
            });

            app.UseSwaggerUIWithRuntimeHandler($"/{swaggerPrefix}/swagger/{Version}/swagger.json", $"{AppName}", c =>
            {
                c.RoutePrefix = $"{swaggerPrefix}/swagger";
            });

            return app;
        }
    }
}