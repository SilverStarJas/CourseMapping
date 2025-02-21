namespace CourseMapping.Domain.Tests.UnitTests
{
    public class UniversityTests
    {
        [Fact]
        public void AddCourse_WhenGivenAnExistingUniversity_SuccessfullyAddedToUniversity()
        {
            // Arrange
            var monash = new University("Monash", "Australia");
            var engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");

            // Act
            monash.AddCourse(engineering);

            // Assert
            Assert.Single(monash.Courses);
        }
    }    
}