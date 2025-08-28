using CourseMapping.Infrastructure.Extensions;
using CourseMapping.Web.Extensions;
using CourseMapping.Web.Middleware;
using Serilog;

namespace CourseMapping.Web;

public class Program
{
    public static void Main(string[] args)
    {
        // Configure Serilog early
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(GetConfiguration())
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting CourseMapping Web Application");
            
            var builder = WebApplication.CreateBuilder(args);

            // Use Serilog as the logging provider
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration));

            // builder.Services.AddOpenTelemetry()
            //     .ConfigureResource(r => r.AddService("CourseMapping"))
            //     .WithTracing(tracing =>
            //     {
            //         tracing.AddSource("CourseMapping.Web");
            //         tracing.AddSource("Example.Source");
            //         tracing.AddAspNetCoreInstrumentation();
            //         tracing.AddHttpClientInstrumentation();
            //         tracing.AddSqlClientInstrumentation();
            //         tracing.AddConsoleExporter();
            //         tracing.AddOtlpExporter(opt =>
            //         {
            //             opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
            //             opt.Protocol = OtlpExportProtocol.HttpProtobuf;
            //             opt.ExportProcessorType = ExportProcessorType.Batch;
            //             opt.BatchExportProcessorOptions = new()
            //             {
            //                 ExporterTimeoutMilliseconds = 5000
            //             };
            //         });
            //     });

            // Add services to the container
            builder.Services.AddWebServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<LoggingMiddleware>();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseMiddleware<AuthenticationMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            Log.Information("Application configured successfully, starting...");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}