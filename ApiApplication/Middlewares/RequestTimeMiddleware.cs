using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace ApiApplication.Middlewares
{
    public interface IHttpRequestTimeFeature
    {
        DateTime RequestTime { get; }
    }

    public class HttpRequestTimeFeature : IHttpRequestTimeFeature
    {
        public DateTime RequestTime { get; }

        public HttpRequestTimeFeature()
        {
            RequestTime = DateTime.Now;
        }
    }

    // You don't need a separate class for this
    public class RequestTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var httpRequestTimeFeature = new HttpRequestTimeFeature();
            context.Features.Set<IHttpRequestTimeFeature>(httpRequestTimeFeature);

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}
