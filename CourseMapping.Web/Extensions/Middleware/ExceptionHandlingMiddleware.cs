using CourseMapping.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Extensions.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CourseSubjectException ex)
            {
                _logger.LogWarning(ex, "Course subject validation failed.");
                await WriteProblemDetails(context, 422, "Course subject validation error", ex.Message);
            }
            catch (SubjectLevelException ex)
            {
                _logger.LogWarning(ex, "Subject level validation failed.");
                await WriteProblemDetails(context, 422, "Subject level validation error", ex.Message);
            }
            catch (UniversityNotFoundException ex)
            {
                _logger.LogWarning(ex, "University not found.");
                await WriteProblemDetails(context, 404, "University not found", ex.Message);
            }
            catch (CourseNotFoundException ex)
            {
                _logger.LogWarning(ex, "Course not found.");
                await WriteProblemDetails(context, 404, "Course not found", ex.Message);
            }
            catch (SubjectNotFoundException ex)
            {
                _logger.LogWarning(ex, "Subject not found.");
                await WriteProblemDetails(context, 404, "Subject not found", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await WriteProblemDetails(context, 500, "Internal Server Error", "An unexpected error occurred.");
            }
        }

        private static async Task WriteProblemDetails(HttpContext context, int statusCode, string title, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
