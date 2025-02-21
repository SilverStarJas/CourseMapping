namespace CourseMapping.Domain;

public class Class1
{
    public void createCourse()
    {
        var monash = new University("Monash", "Australia");
        var engineering = new Course("Engineering", "Four year course for Engineering", "ENG3000");

        monash.AddCourse(engineering);

        var algorithms = new Unit("Algorithms", "FIT2004", "Introduction to algorithms", 2);
        var abc = new Unit("ABC", "ABC1234", "Test unit", 1);

        engineering.AddUnit(algorithms);
        engineering.AddUnit(abc);

        var engineeringUnits = engineering.Units;
    }
}
