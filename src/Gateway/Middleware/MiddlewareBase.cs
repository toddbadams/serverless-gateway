using System.Threading.Tasks;
using Gateway.Application.Pipelines;

namespace Gateway.Middleware
{
    public abstract class MiddlewareBase
    {
        public abstract Task InvokeAsync(Context httpContext);
        public MiddlewareBase Next { get; set; }
    }
}