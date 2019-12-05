using System.Threading.Tasks;
using Gateway.Application.Pipelines;
using Gateway.Application.Secrets;

namespace Gateway.Middleware
{
    public class FunctionHostKeyMiddleware : MiddlewareBase
    {
        private const string FunctionHostHeaderKey = "x-functions-key";
        private readonly ISecretsProvider _secretsProvider;

        public FunctionHostKeyMiddleware(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
        }

        public override async Task InvokeAsync(Context httpContext)
        {
            var key = await _secretsProvider.Get($"{httpContext.DownstreamKey}-functionHostKey");
            httpContext.DownstreamRequest.Headers.Add(FunctionHostHeaderKey, key);
        }
    }
}