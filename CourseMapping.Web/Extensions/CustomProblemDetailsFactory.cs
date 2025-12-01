using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace CourseMapping.Web.Extensions;

public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options.Value;
    }

    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        statusCode ??= 500;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyCustomProblemDetails(problemDetails, statusCode);
        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        statusCode ??= 400;
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyCustomProblemDetails(problemDetails, statusCode);
        return problemDetails;
    }

    private void ApplyCustomProblemDetails(ProblemDetails problemDetails, int? statusCode)
    {
        switch (statusCode)
        {
            case 422:
                problemDetails.Title = "Unprocessable Entity";
                problemDetails.Type = "https://tools.ietf.org/html/rfc4918#section-11.2";
                problemDetails.Detail = "The request was well-formed but was unable to be followed due to semantic errors. Please check your input and try again.";
                break;
            case 404:
                problemDetails.Title ??= "Not Found";
                problemDetails.Type ??= "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                problemDetails.Detail ??= "The specified resource was not found.";
                break;
        }
    }
}

