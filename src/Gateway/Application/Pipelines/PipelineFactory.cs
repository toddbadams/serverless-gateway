using Gateway.Middleware;

namespace Gateway.Application.Pipelines
{
    public class PipelineFactory : IPipelineFactory
    {
        private readonly IMiddlewareFactory _middlewareFactory;
        private readonly IContextFactory _contextFactory;

        public PipelineFactory(IMiddlewareFactory middlewareFactory,
            IContextFactory contextFactory)
        {
            _middlewareFactory = middlewareFactory;
            _contextFactory = contextFactory;
        }
        public AuthorizedPipeline Authorized() => new AuthorizedPipeline(_contextFactory, _middlewareFactory);
    }
}