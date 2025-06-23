using CourseMapping.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CourseMapping.Tests.IntegrationTests;

internal class WebApplicationFactory : WebApplicationFactory<Web.Program>
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
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("CourseMappingDb");
        
        return connectionString;
    }
}