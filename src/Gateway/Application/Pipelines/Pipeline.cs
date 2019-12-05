using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gateway.Middleware;

namespace Gateway.Application.Pipelines
{
    public class Pipeline
    {
        private readonly IContextFactory _contextFactory;

        private readonly List<MiddlewareBase> _pipeline;

        internal Pipeline(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _pipeline = new List<MiddlewareBase>();
        }

        protected void Register(MiddlewareBase middlewareBase)
        {
            _pipeline.Add(middlewareBase);
        }

        public async Task<HttpResponseMessage> ExecuteAsync(string key, HttpRequestMessage req)
        {
            if (!_pipeline.Any()) throw new MiddlewareException();

            var context = _contextFactory.Create(key, req);

            try
            {
                foreach (var middleware in _pipeline)
                {
                    await middleware.InvokeAsync(context);
                }
            }
            catch (Exception e)
            {
                // log the exception
                return context?.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

            return await context.SendAsync();
        }
    }
}