namespace SRMP.Middelware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unexpected error occurred.");

                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException:
                    statusCode =
                        StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;

                case KeyNotFoundException:
                    statusCode =
                        StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;

                case FileNotFoundException:
                    statusCode =
                        StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode =
                        StatusCodes.Status403Forbidden;
                    message = exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode =
                        StatusCodes.Status409Conflict;
                    message = exception.Message;
                    break;

                default:
                    statusCode =
                        StatusCodes.Status500InternalServerError;

                    message =
                        "An unexpected server error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType =
                "application/json";

            await context.Response.WriteAsJsonAsync(
                new
                {
                    message = message
                });
        }
    }
}