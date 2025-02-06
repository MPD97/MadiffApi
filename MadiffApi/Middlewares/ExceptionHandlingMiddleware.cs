using MadiffApi.Exceptions;
using MadiffApi.Responses;
using System.Net;
using System.Text.Json;

namespace MadiffApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred");
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            ErrorResponse errorResponse;

            switch (exception)
            {
                case ApiException apiException:
                    response.StatusCode = apiException.StatusCode;

                    errorResponse = new ErrorResponse(apiException.Message, context.TraceIdentifier);

                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    errorResponse = new ErrorResponse(_environment.IsDevelopment()
                        ? exception.Message
                        : "An internal server error occurred", context.TraceIdentifier);
                   
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
}
