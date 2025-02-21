namespace CourseMapping.Domain.Tests.UnitTests
{
    public class CourseTests
    {
        [Fact]
        public void AddUnit_WhenGivenAnExistingCourse_SuccessfullyAddedToCourse()
        {
            // Arrange
            Course engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");
            Unit abc = new Unit("ABC", "ABC1234", "Test unit", 1);

            // Act
            engineering.AddUnit(abc);

            // Assert
            Assert.Single(engineering.Units);
        }

        [Fact]
        public void AddUnit_WhenThereAreThreeUnits_Error()
        {
            // Arrange
            Course engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");
            Unit abc = new Unit("ABC", "ABC1234", "Test unit", 1);
            Unit algorithms = new Unit("Algorithms", "FIT2004", "Introduction to algorithms", 2);
            Unit databases = new Unit("Databases", "FIT3171", "Introduction to Databases", 3);

            engineering.AddUnit(abc);
            engineering.AddUnit(algorithms);
            engineering.AddUnit(databases);

            Unit fourth = new Unit("Test", "FITTest", "Test unit", 4);

            // Act
            engineering.AddUnit(fourth);

            // Assert
            Assert.Equal(3, engineering.Units.Count);
        }
    }
}