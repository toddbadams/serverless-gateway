using System.Net.Http;
using System.Threading.Tasks;
using Gateway.Application.Pipelines;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Samples
{
    public class WinesApi
    {
        private const string Name = "wines";
        private readonly Pipeline _pipeline;

        public WinesApi(IPipelineFactory pipelineFactory)
        {
            _pipeline = pipelineFactory.Authorized();
        }

        [FunctionName(Name + "-get")]
        public async Task<HttpResponseMessage> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Name + "/{wineId?}")]
            HttpRequestMessage req) => await _pipeline.ExecuteAsync(Name, req);

        [FunctionName(Name + "-post")]
        public async Task<HttpResponseMessage> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Name)]
            HttpRequestMessage req) => await _pipeline.ExecuteAsync(Name, req);

        [FunctionName(Name + "-put")]
        public async Task<HttpResponseMessage> Put(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Name + "/{wineId}")]
            HttpRequestMessage req) => await _pipeline.ExecuteAsync(Name, req);

        [FunctionName(Name + "-delete")]
        public async Task<HttpResponseMessage> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "delete", Route = Name + "/{wineId}")]
            HttpRequestMessage req) => await _pipeline.ExecuteAsync(Name, req);
    }
}