namespace CourseMapping.Domain.Exceptions;

public class CourseException : Exception
{
    public CourseException() { }

    public CourseException(string message)
        : base(message) { }
}
