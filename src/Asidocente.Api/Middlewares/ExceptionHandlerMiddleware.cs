using Asidocente.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Asidocente.Api.Middlewares;

/// <summary>
/// Global exception handler middleware
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var responseModel = new ErrorResponse
        {
            Message = exception.Message,
            Details = exception.StackTrace
        };

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = "Validation failed";
                responseModel.Errors = validationException.Errors;
                break;

            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing your request";
                responseModel.Details = null; // Hide stack trace in production
                break;
        }

        var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
