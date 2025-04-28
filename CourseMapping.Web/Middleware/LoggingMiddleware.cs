namespace CourseMapping.Web.Middleware;

public class LoggingMiddleware : IMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var keyValue = $"{context.Request.Method} {context.Request.Path}";

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogDebug(keyValue, "Request: {Request}", context.Request);
        }
        
        await next(context);
    }
}