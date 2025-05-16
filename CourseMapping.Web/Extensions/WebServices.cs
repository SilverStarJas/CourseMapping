using CourseMapping.Web.Middleware;
using CourseMapping.Web.Middleware.Authentication;

namespace CourseMapping.Web.Extensions;

public static class WebServices
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddTransient<LoggingMiddleware>();
        services.AddTransient<AuthenticationMiddleware>();
    }
}