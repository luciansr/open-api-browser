using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Models.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            { 
                return;
            }
            
            var excludedProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);
            foreach (PropertyInfo excludedProperty in excludedProperties)
            {
                var keyToLower = excludedProperty.Name.ToLowerInvariant();
                var key = excludedProperty.Name;
                
                if (schema.Properties.ContainsKey(keyToLower))
                {
                    schema.Properties.Remove(keyToLower);
                }
                
                if (schema.Properties.ContainsKey(key))
                {
                    schema.Properties.Remove(key);
                }
            }
        }
    }
}