using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gateway.Application.Configuration;

namespace Gateway.Application.Pipelines
{
    public class Context
    {
        private readonly IConfigurationProvider _configurationProvider;

        public Context(HttpRequestMessage request, IConfigurationProvider configurationProvider,
            string key, HttpClient httpClient)
        {
            _configurationProvider = configurationProvider;
            Request = request;
            DownstreamRequest = new HttpRequestMessage(request.Method, Uri);
            Properties = new Dictionary<string, string>();
            Properties.Add("Method", request.Method.ToString());
            if (request.Headers.TryGetValues("ContentType", out var values))
            {
                Properties.Add("ContentType", string.Join(",", values.ToArray()));
            }
            //Properties.Add("RequestBody", requestBody);
            DownstreamKey = key;
            HttpClient = httpClient;
        }

        public HttpRequestMessage Request { get; }
        public HttpRequestMessage DownstreamRequest { get; }
        public IDictionary<string, string> Properties { get; }
        public string DownstreamKey { get; }
        public HttpClient HttpClient { get; }

        public async Task<HttpResponseMessage> SendAsync()
        {
            return await HttpClient.SendAsync(DownstreamRequest);
        }

        private string Uri =>
            $"{Value("scheme")}://{Value("host")}:{Value("port")}{Value("route")}";

        private string Value(string name) => _configurationProvider.Get($"{DownstreamKey}-{name}");
    }
}