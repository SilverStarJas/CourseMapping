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
        var startTime = DateTime.UtcNow;
        var requestId = Guid.NewGuid();
        
        // Add request ID to log context for correlation
        using (_logger.BeginScope(new Dictionary<string, object> { ["RequestId"] = requestId }))
        {
            try
            {
                await next(context);

                var duration = DateTime.UtcNow - startTime;
                
                // Log successful requests
                _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {Duration}ms", 
                    context.Request.Method, 
                    context.Request.Path, 
                    context.Response.StatusCode,
                    (int)duration.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime;
                
                // Log failed requests
                _logger.LogError(ex, "HTTP {Method} {Path} failed with {StatusCode} after {Duration}ms", 
                    context.Request.Method, 
                    context.Request.Path, 
                    context.Response.StatusCode,
                    (int)duration.TotalMilliseconds);
                throw;
            }
        }
    }
}