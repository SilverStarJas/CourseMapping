using Microsoft.AspNetCore.Mvc;

namespace CourseMapping.Web.Extensions.Results;

public class NotFoundObjectResult : ObjectResult
{
    public NotFoundObjectResult(string detail, string? instance = null)
        : base(new ProblemDetails
        {
            Title = "Not Found",
            Status = StatusCodes.Status404NotFound,
            Detail = detail,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Instance = instance
        })
    {
        StatusCode = StatusCodes.Status404NotFound;
        ContentTypes.Add("application/problem+json");
    }
}

