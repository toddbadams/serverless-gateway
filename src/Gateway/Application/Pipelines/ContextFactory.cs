using System.Net.Http;
using Gateway.Application.Configuration;

namespace Gateway.Application.Pipelines
{
    public class ContextFactory : IContextFactory
    {
        private readonly IConfigurationProvider _config;
        private readonly HttpClient _httpClient;

        public ContextFactory(IConfigurationProvider config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public Context Create(string key, HttpRequestMessage req) =>
            new Context(req, _config, key, _httpClient);
    }
}