using Serilog;
using System.Diagnostics;

namespace SmartSchool.Api.Middlewares
{
    public class LoggingMiddleware: IMiddleware
    {
       

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = context.TraceIdentifier;
            var sw = Stopwatch.StartNew();
            Log.Information("Handling HTTP request {Method} {Path} | CorrelationId: {CorrelationId}", context.Request.Method, context.Request.Path, correlationId);

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception in LoggingMiddleware | CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
            finally
            {
                sw.Stop();

                Log.Information("HTTP Response {StatusCode} {Method} {Path} in {Elapsed}ms | CorrelationId: {CorrelationId}",
                                context.Response.StatusCode,
                                context.Request.Method,
                                context.Request.Path,
                                sw.ElapsedMilliseconds,
                                correlationId);
            }

        }

    }
}
