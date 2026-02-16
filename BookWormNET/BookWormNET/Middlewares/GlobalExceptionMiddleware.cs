using BookWormNET.DTO;
using BookWormNET.Exceptions;
using System.Net;
using System.Text.Json;

namespace BookWormNET.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred";

            if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            var response = new ErrorResponseDTO
            {
                StatusCode = statusCode,
                Message = message,
                Details = exception.InnerException?.Message,
                TimeStamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
