using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Models.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwaggerRuntimeModels.Swagger;

namespace Services.OpenApi
{
    public class OpenApiUpdater : ISwaggerRuntimeUpdater
    {
        private readonly OpenApiRepository _openApiRepository;

        public OpenApiUpdater(OpenApiRepository openApiRepository)
        {
            _openApiRepository = openApiRepository;
        }
        
        public Task<IEnumerable<OpenApiSummary>> GetUpdatedOpenApiList(CancellationToken cancellationToken)
        {
            return _openApiRepository.GetAllApiDefinitions(cancellationToken);
        }

        public Task<OpenApiDefinition> GetOpenApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken)
        {
            return _openApiRepository.GetApiDefinition(workspace, service, version, cancellationToken);
        }
    }
}