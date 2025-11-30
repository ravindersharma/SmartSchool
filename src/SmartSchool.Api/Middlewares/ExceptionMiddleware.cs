using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Api.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var response = new { success = false, error = ex.Message };

                await context.Response.WriteAsJsonAsync(response);

            }
        }
    }
}
