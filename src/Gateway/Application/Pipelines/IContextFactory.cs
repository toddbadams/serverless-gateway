using System.Net.Http;

namespace Gateway.Application.Pipelines
{
    public interface IContextFactory
    {
        Context Create(string key, HttpRequestMessage req);
    }
}