using Microsoft.AspNetCore.Http.Extensions;

namespace CourseMapping.Web.Middleware.Authentication;

public class AuthenticationMiddleware : IMiddleware
{
    private readonly IConfiguration _configuration;

    public AuthenticationMiddleware(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.GetDisplayUrl().Contains("openapi"))
        {
            await next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(("API Key is missing."));
            return;
        }

        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
        if (apiKey != null && !apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(("API Key is invalid."));
            return;
        }
        
        await next(context);
    }
}