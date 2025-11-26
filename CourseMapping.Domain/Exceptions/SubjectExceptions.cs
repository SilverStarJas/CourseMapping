namespace CourseMapping.Domain.Exceptions;

public class SubjectException : Exception
{
    public SubjectException() { }

    public SubjectException(string message)
        : base(message) { }
}