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
        var requestDetails = $"REQUEST: {context.Request.Method} at {context.Request.Path}";

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{RequestDetails}", requestDetails);
        }
    
        await next(context);
    }
}