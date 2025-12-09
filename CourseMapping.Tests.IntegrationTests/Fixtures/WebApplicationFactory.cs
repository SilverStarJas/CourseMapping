using CourseMapping.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CourseMapping.Tests.IntegrationTests.Fixtures;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            
            var connectionString = GetConnectionString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
        });
    }
    
    private static string? GetConnectionString()
    {
        // Check environment variable first (for CI/CD)
        var envConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CourseMappingDb");
        if (!string.IsNullOrEmpty(envConnectionString))
        {
            return envConnectionString;
        }
        
        // Fall back to appsettings.json (for local development)
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("CourseMappingDb");
        
        return connectionString;
    }
}