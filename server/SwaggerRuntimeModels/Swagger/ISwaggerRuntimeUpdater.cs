using System.Collections.Generic;
using System.Threading.Tasks;
using Models.OpenApi;

namespace SwaggerRuntimeModels.Swagger
{
    public interface ISwaggerRuntimeUpdater
    {
        Task<IEnumerable<OpenApiSummary>> GetUpdatedOpenApiList();
        Task<OpenApiDefinition> GetOpenApiDefinition(string workspace, string service, string version);
    }
}