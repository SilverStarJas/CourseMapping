using CourseMapping.Infrastructure;
using CourseMapping.Infrastructure.Extensions;
using CourseMapping.Web.Extensions;
using CourseMapping.Web.Middleware;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq(builder.Configuration.GetSection("Seq"));

builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder
    .Services
    .AddOpenTelemetry()
    .ConfigureResource(x => x.AddService("CourseMapping"))
    .WithTracing(c =>
    {
        c.AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddNpgsql();
    })
    .UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:5100/ingest/otlp/v1/traces"));

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Expire1Minutes", policyBuilder => policyBuilder.Expire(TimeSpan.FromMinutes(1)));
});

builder.Services.AddProblemDetails();

var app = builder.Build();

// Register custom exception handling middleware
app.UseMiddleware<CourseMapping.Web.Extensions.Middleware.ExceptionHandlingMiddleware>();
app.UseStatusCodePages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseOutputCache();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Make Program class accessible to test projects
public partial class Program { }