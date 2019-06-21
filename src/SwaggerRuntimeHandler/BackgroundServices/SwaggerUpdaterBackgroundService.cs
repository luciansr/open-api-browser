using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SwaggerRuntimeHandler.Swagger;

namespace SwaggerRuntimeHandler.BackgroundServices
{
    public class SwaggerUpdaterBackgroundService : BackgroundService
    {
        private readonly SwaggerUIRuntimeHandler _swaggerUiRuntimeHandler;

        public SwaggerUpdaterBackgroundService(SwaggerUIRuntimeHandler swaggerUiRuntimeHandler)
        {
            _swaggerUiRuntimeHandler = swaggerUiRuntimeHandler;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () => await RunBackgroundTaskAsync(stoppingToken), stoppingToken);
        }
        
        private async Task RunBackgroundTaskAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _swaggerUiRuntimeHandler.UpdateEndpoints(stoppingToken);
                await Task.Delay(SwaggerUIRuntimeHandler.UpdateInterval, stoppingToken);
            }
        }
    }
}