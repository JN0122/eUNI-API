using System.Net;
using System.Text.Json;
using eUNI_API.Exception;

namespace eUNI_API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpCustomException ex)
        {
            await HandleHttpCustomExceptionAsync(context, ex);
        }
        catch (System.Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleHttpCustomExceptionAsync(HttpContext context, HttpCustomException exception)
    {
        return ReturnExceptionAsync(context, exception.StatusCode, exception.Message);
    }

    private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        const string message = "An unexpected error occurred.";
        
        return ReturnExceptionAsync(context, (int)statusCode, message);
    }

    private static Task ReturnExceptionAsync(HttpContext context, int statusCode, string message)
    {
        var result = JsonSerializer.Serialize(new { message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(result);
    }
}