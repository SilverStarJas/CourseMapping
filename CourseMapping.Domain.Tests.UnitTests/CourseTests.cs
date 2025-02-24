namespace CourseMapping.Domain.Tests.UnitTests
{
    public class CourseTests
    {
        [Fact]
        public void AddUnit_WhenGivenAnExistingCourse_SuccessfullyAddedToCourse()
        {
            // Arrange
            Course engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");
            Subject abc = new Subject("ABC", "ABC1234", "Test unit", 1);

            // Act
            engineering.AddSubjects(abc);

            // Assert
            Assert.Single(engineering.Subjects);
        }

        [Fact]
        public void AddUnit_WhenThereAreThreeUnits_OnlyThreeMax()
        {
            // Arrange
            Course engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");
            Subject abc = new Subject("ABC", "ABC1234", "Test unit", 1);
            Subject algorithms = new Subject("Algorithms", "FIT2004", "Introduction to algorithms", 2);
            Subject databases = new Subject("Databases", "FIT3171", "Introduction to Databases", 3);

            // Act
            engineering.AddSubjects(abc);
            engineering.AddSubjects(algorithms);
            engineering.AddSubjects(databases);

            // Assert
            Assert.Equal(3, engineering.Subjects.Count);
        }

        [Fact]
        public void AddUnit_WhenThereAreMoreThanThreeUnits_ThrowException()
        {
            // Arrange
            Course engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");
            Subject abc = new Subject("ABC", "ABC1234", "Test unit", 1);
            Subject algorithms = new Subject("Algorithms", "FIT2004", "Introduction to algorithms", 2);
            Subject databases = new Subject("Databases", "FIT3171", "Introduction to Databases", 3);
            Subject fourth = new Subject("Test", "FITTest", "Test unit", 4);

            engineering.AddSubjects(abc);
            engineering.AddSubjects(algorithms);
            engineering.AddSubjects(databases);

            // Act and Assert
            var exception = Assert.Throws<InvalidOperationException>(() => engineering.AddSubjects(fourth));

            Assert.Equal("Up to three subjects allowed.", exception.Message);

        }
    }
}