namespace CourseMapping.Domain.Tests.UnitTests
{
    public class SubjectTests
    {
        [Fact]
        public void CreateSubject_WhenGivenHighestLevel_SuccessfullyCreateSubject()
        {
            // Arrange and Act
            var abc = new Subject("ABC", "ABC1234", "Test unit", 1);

            // Assert 
            Assert.InRange(abc.Level, 1, 5);
        }

        [Fact]
        public void CreateSubject_WhenGivenLowestLevel_SuccessfullyCreateSubject()
        {
            // Arrange and Act
            var def = new Subject("DEF", "DEF5678", "Second year test", 5);

            // Assert 
            Assert.InRange(def.Level, 1, 5);
        }

        [Fact]
        public void CreateSubject_WhenGivenLevelTooHigh_ThrowExceptioon()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Subject("GHI", "GHI9012", "Higher than level 5", 6);
            });

            Assert.Equal("Subject level must be between 1 and 5.", exception.ParamName);
        }

        [Fact]
        public void CreateSubject_WhenGivenLevelTooLow_ThrowExceptioon()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Subject("JKL", "JKL3456", "Level is less than 1", 0);
            });

            Assert.Equal("Subject level must be between 1 and 5.", exception.ParamName);
        }

        [Fact]
        public void UpdateDescription_WhenGivenValidSubject_SuccessfullyUpdated()
        {
            // Arrange
            var mno = new Subject("MNO", "MNO7890", "Update description test", 2);

            // Act
            mno.UpdateDescription("Testing that the description is successful");

            // Assert
            Assert.Equal("Testing that the description is successful", mno.Description);
        }
    }
}

