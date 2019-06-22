using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Models.Config;
using Models.OpenApi;
using Newtonsoft.Json;

namespace Services.OpenApi
{
    public class OpenApiRepository
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly AwsConfig _awsConfig;

        public OpenApiRepository(IAmazonS3 amazonS3, AwsConfig awsConfig)
        {
            _amazonS3 = amazonS3;
            _awsConfig = awsConfig;
        }

        private string GetKey(string workspace, string service, string version) => $"{workspace}/{service}/{version}.json";
        
        public async Task SaveApiDefinition(string workspace, string service, string version, OpenApiDefinition openApiDefinition, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(openApiDefinition));
                writer.Flush();
                stream.Position = 0;

                await _amazonS3.PutObjectAsync(new PutObjectRequest
                {
                    ContentType = "application/json",
                    InputStream = stream,
                    BucketName = _awsConfig.S3Bucket,
                    Key = GetKey(workspace, service, version)
                }, cancellationToken);
            }
        }

        public async Task<OpenApiDefinition> GetApiDefinition(string workspace, string service, string version, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _amazonS3.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = _awsConfig.S3Bucket,
                    Key = GetKey(workspace, service, version)
                }, cancellationToken);
                var stream = response.ResponseStream;
                
                if (stream == null) return null;
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    var jsonStr = Encoding.UTF8.GetString(ms.ToArray());
                    var result = JsonConvert.DeserializeObject<OpenApiDefinition>(jsonStr);
                    return result;
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                if (s3Exception.StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw;
            }
        }

        public async Task<IEnumerable<OpenApiSummary>> GetAllApiDefinitions(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var keys = await _amazonS3.GetAllObjectKeysAsync(_awsConfig.S3Bucket, "", null);
            
            return keys.Select(key =>
            {
                var keySplit = key.Split(new[] {"/", "."}, StringSplitOptions.None);
                var workspace = keySplit[0];
                var service = keySplit[1];
                var version = keySplit[2];

                return new OpenApiSummary
                {
                    Workspace = workspace,
                    ServiceName = service,
                    Version = version
                };
            }).ToList();
        }
    }
}
