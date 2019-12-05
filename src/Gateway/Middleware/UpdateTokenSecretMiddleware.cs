using System.Threading.Tasks;
using Gateway.Application.Pipelines;
using Gateway.Application.Secrets;

namespace Gateway.Middleware
{
    public class UpdateTokenSecretMiddleware : MiddlewareBase
    {
        
        private const string UpdateTokenSecretHeaderKey = "x-update-token-secret";
        private readonly ISecretsProvider _secretsProvider;

        public UpdateTokenSecretMiddleware(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
        }

        public override async Task InvokeAsync(Context httpContext)
        {
            var key = await _secretsProvider.Get("updateTokenSecret");
            httpContext.DownstreamRequest.Headers.Add(UpdateTokenSecretHeaderKey, key);
        }
    }
}