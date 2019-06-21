using Newtonsoft.Json.Linq;

namespace Models.OpenApi
{
    public class OpenApiDefinition
    {
        public string Workspace { get; set; }
        public string ServiceName { get; set; }
        public string Version { get; set; }
        public JObject Schema { get; set; }
    }
}