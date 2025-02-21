namespace CourseMapping.Domain.Tests.UnitTests
{
    // Subject = Unit; renamed to avoid confusion
    public class SubjectTests
    {
        [Fact]
        public void CreateUnit_WhenGivenValidArgs_SuccessfullyCreateUnit()
        {
            // Arrange
            var abc = new Unit("ABC", "ABC1234", "Test unit", 1);
        }
    }
}
