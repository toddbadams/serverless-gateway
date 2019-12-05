namespace Gateway.Application.Pipelines
{
    public interface IPipelineFactory
    {
        AuthorizedPipeline Authorized();
    }
}