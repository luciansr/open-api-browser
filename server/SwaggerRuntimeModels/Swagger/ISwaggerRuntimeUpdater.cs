using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.OpenApi;

namespace SwaggerRuntimeModels.Swagger
{
    public interface ISwaggerRuntimeUpdater
    {
        Task<IEnumerable<OpenApiSummary>> GetUpdatedOpenApiList(CancellationToken cancellationToken);
        Task<OpenApiDefinition> GetOpenApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken);
    }
}