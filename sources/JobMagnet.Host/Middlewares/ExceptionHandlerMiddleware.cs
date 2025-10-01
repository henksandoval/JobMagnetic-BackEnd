using System.Net;
using System.Text.Json;
using JobMagnet.Infrastructure.Exceptions;

namespace JobMagnet.Host.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = "An internal server error has occurred.";
        
        if (exception is InvalidCredentialsAdapterException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            message = "Incorrect email or password.";
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = message });
        
        return context.Response.WriteAsync(result);
    }
}