namespace CourseMapping.Domain.Exceptions;

public class CourseNotFoundException : Exception
{
    public CourseNotFoundException(string message) : base(message) {}
}

