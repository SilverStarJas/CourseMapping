using CourseMapping.Infrastructure.Extensions;
using CourseMapping.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.Use(async (context, next) =>
{
    await next.Invoke(); 
    // Other delegates(?): References to methods with a param list and return type
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
