using CourseMapping.Infrastructure.Extensions;
using CourseMapping.Web.Extensions;
using CourseMapping.Web.Middleware;
using CourseMapping.Web.Middleware.Authentication;

namespace CourseMapping.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddWebServices();
        builder.Logging.AddConsole();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<LoggingMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<AuthenticationMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}