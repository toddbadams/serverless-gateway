using Gateway.Application.Secrets;

namespace Gateway.Middleware
{
    class MiddlewareFactory : IMiddlewareFactory
    {
        readonly ISecretsProvider _secretsProvider;

        public MiddlewareFactory(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
        }

        public CorrelationIdMiddleware CorrelationId() => new CorrelationIdMiddleware();
        public FunctionHostKeyMiddleware FunctionHostKey() => new FunctionHostKeyMiddleware(_secretsProvider);
        public UpdateTokenSecretMiddleware UpdateTokenSecret() => new UpdateTokenSecretMiddleware(_secretsProvider);
    }
}
