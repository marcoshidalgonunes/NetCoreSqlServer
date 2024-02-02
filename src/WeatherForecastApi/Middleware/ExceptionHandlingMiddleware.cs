using System.Net;
using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;

namespace WeatherForecastApi.Middleware;

/// <summary>
/// Adapted from https://dev.to/andytechdev/aspnet-core-middleware-working-with-global-exception-handling-3hi0
/// </summary>

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
        _logger.LogError(exception, "An unexpected error occurred.");

        //More log stuff        

        ExceptionResponse response = exception switch
        {
            ApplicationException _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Application exception occurred."),
            KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
            DbUpdateException _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.InnerException.Message),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}