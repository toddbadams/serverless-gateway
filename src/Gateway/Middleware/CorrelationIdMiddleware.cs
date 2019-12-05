using System;
using System.Threading.Tasks;
using Gateway.Application.Pipelines;

namespace Gateway.Middleware
{
    public class CorrelationIdMiddleware : MiddlewareBase
    {
        private const string Header = "Request-Id";
        public override Task InvokeAsync(Context httpContext)
        {
            var headers = httpContext.Request.Headers;
            if (!headers.Contains(Header) || headers.GetValues(Header) == null)
            {
                httpContext.Request.Headers.Add(Header, Guid.NewGuid().ToString());
            }
            //httpContext.Properties.Add("Request-Id", httpContext.Request.Headers.GetValues("Request-Id").First());
            return Task.Delay(0);
        }
    }
}