using Microsoft.AspNetCore.Http;

namespace CourseMapping.Infrastructure.Middleware;

public class LoggingMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        throw new NotImplementedException();
    }
}