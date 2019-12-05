namespace Gateway.Middleware
{
    public interface IMiddlewareFactory
    {
        CorrelationIdMiddleware CorrelationId();
        FunctionHostKeyMiddleware FunctionHostKey();
        UpdateTokenSecretMiddleware UpdateTokenSecret();
    }
}