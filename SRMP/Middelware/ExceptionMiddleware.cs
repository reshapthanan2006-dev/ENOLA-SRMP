using System.Net;
using System.Text.Json;

namespace SRMP.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                UnauthorizedAccessException
                    => HttpStatusCode.Unauthorized,

                KeyNotFoundException
                    => HttpStatusCode.NotFound,

                InvalidOperationException
                    => HttpStatusCode.BadRequest,

                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                message = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred."
                    : exception.Message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}