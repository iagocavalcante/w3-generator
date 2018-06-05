using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Http.Extensions;

namespace W3TraceCore
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TraceMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Trace trace)
        {
            try
            {
                using (var requestBodyStream = new MemoryStream())
                {
                    var originalRequestBody = context.Request.Body;

                    await context.Request.Body.CopyToAsync(requestBodyStream);
                    requestBodyStream.Seek(0, SeekOrigin.Begin);
                    var url = UriHelper.GetDisplayUrl(context.Request);
                    var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
                    trace.GravarTrace($" REQUEST METHOD: {context.Request.Method}\nREQUEST URL: {url}\nREQUEST BODY:\n{requestBodyText}", 2);
                    requestBodyStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = requestBodyStream;

                    using (var responseBodyStream = new MemoryStream())
                    {
                        var bodyStream = context.Response.Body;
                        context.Response.Body = responseBodyStream;

                        await _next(context);
                        context.Request.Body = originalRequestBody;


                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                        if (context.Request.Path.ToString().Contains("/api/"))
                            trace.GravarTrace($" RESPONSE CODE: {context.Response.StatusCode}\nRESPONSE BODY:\n{responseBody}", 3);

                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        await responseBodyStream.CopyToAsync(bodyStream);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "";
                var innerEx = "";

                try
                {
                    // CASO TENHA MAIS DE UMA EXCEÇÃO
                    var innerExceptions = ((AggregateException)ex).InnerExceptions;
                    var innerException = innerExceptions[0];
                    msg = innerException.Message;
                    innerEx = Trace.GetInnerExceptions(ex);
                }
                catch
                {
                    msg = ex.Message;
                    innerEx = Trace.GetInnerExceptions(ex);
                }
                if (context.Response.HasStarted)
                {
                    throw;
                }
                trace.GravarTrace($" MESSAGE: {msg}\nINNEREXCEPTION:\n{innerEx}\n\nSTACKTRACE:\n{ex.StackTrace}", 1);
                context.Response.StatusCode = 500;
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TraceMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceMiddleware>();
        }
    }
}
