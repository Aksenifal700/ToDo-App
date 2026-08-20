using System.Net;
using System.Text.Json;
using TodoApp.API.Models.Response;
using TodoApp.BusinessLogic.Exceptions;

namespace TodoApp.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        var response = exception switch
        {
            AppException appEx => new ExceptionResponse(appEx.StatusCode, appEx.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error")
        };

        if (exception is AppException)
            _logger.LogWarning(exception, "Business rule violation: {Message}", exception.Message);
        else
            _logger.LogError(exception, "Unhandled exception occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}