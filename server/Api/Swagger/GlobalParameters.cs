using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger
{
    public class GlobalParameters: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

//            operation.Parameters.Add(new OpenApiParameter
//            {
//                Name = "global-parameter",
//                In = ParameterLocation.Query,
//                Description = "Account",
//                Schema = new OpenApiSchema(),
//                Required = true
//            });
//            
//            operation.Parameters.Add(new OpenApiParameter
//            {
//                Name = "X-Global-Header",
//                In = ParameterLocation.Header,
////                Description = "App key",
//                Schema = new OpenApiSchema(),
//                Required = true
//            });
        }
    }
}