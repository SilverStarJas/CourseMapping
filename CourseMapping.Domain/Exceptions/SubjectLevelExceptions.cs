namespace CourseMapping.Domain.Exceptions;

/// <summary>
/// Exception thrown when a Subject level validation fails.
/// </summary>
public class SubjectLevelException : Exception
{
    public SubjectLevelException()
    {
    }

    public SubjectLevelException(string message) 
        : base(message)
    {
    }
}