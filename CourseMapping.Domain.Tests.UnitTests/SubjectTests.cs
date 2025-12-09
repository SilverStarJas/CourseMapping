using CourseMapping.Domain.Exceptions;

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
            var level = 6;
            var exception = Assert.Throws<SubjectLevelException>(() =>
            {
                var subject = new Subject("GHI", "GHI9012", "Higher than level 5", level);
            });

            Assert.Equal($"Invalid subject level: {level}. Level must be betwen 1 and 5.", exception.Message);
        }

        [Fact]
        public void CreateSubject_WhenGivenLevelTooLow_ThrowExceptioon()
        {
            var level = 0;
            var exception = Assert.Throws<SubjectLevelException>(() =>
            {
                var subject = new Subject("JKL", "JKL3456", "Level is less than 1", level);
            });

            Assert.Equal($"Invalid subject level: {level}. Level must be between 1 and 5.", exception.Message);
        }
    }
}
