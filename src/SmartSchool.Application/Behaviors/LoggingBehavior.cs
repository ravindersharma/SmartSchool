using MediatR;
using Serilog;
using System.Diagnostics;

namespace SmartSchool.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var requestName = typeof(TRequest).Name;
            var correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            var sw = Stopwatch.StartNew();

            Log.Information("Handling {RequestName} with {@Request} | CorrelationIdL {CorrelationId}", requestName, request, correlationId);


            try
            {
                var response = await next();
                sw.Stop();

                Log.Information("Handled {RequestName} in {ElapsedMilliseconds}ms | {@Response} | CorrelationId: {CorrelationId}", requestName, sw.ElapsedMilliseconds, response, correlationId);

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log.Error(ex, "Error handling {RequestName} after {ElapsedMilliseconds}ms | CorrelationId: {CorrelationId}", requestName, sw.ElapsedMilliseconds, correlationId);

                throw;

            }


        }
    }
}
