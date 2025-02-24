namespace CourseMapping.Domain;

public class Class1
{
    public void createCourse()
    {
        var monash = new University("Monash", "Australia");
        var engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");

        monash.AddCourse(engineering);

        var algorithms = new Subject("Algorithms", "FIT2004", "Introduction to algorithms", 2);
        var abc = new Subject("ABC", "ABC1234", "Test unit", 1);

        engineering.AddSubjects(algorithms);
        engineering.AddSubjects(abc);

        var engineeringUnits = engineering.Subjects;
    }
}
