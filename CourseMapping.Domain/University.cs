namespace CourseMapping.Domain
{
    public class University
    {
        // Properties
        public Guid Id { get; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<Course> Courses { get; }

        // Constructor
        public University(Guid id, string name, string country)
        {
            Id = id;
            Name = name;
            Country = country;
            Courses = [];
        }

        public void UpdateUniversity(string? name, string? country)
        {
            if (name != null)
                Name = name;

            if (country != null)
                Country = country;
        }

        public void AddCourse(Course newCourse)
        {
            Courses.Add(newCourse);
        }

        public void RemoveCourse(string courseCode)
        {
            var course = Courses.FirstOrDefault(x => x.Code == courseCode);

            if (course is null)
                return;

            Courses.Remove(course);
        }

        public List<string> MapCurrentSubjectsToNew(List<Subject> currentSubjects, List<Subject> allSubjects)
        {
            var matchedSubjectsCodes = new List<string>();

            foreach (var subject in currentSubjects)
            {
                var matches = allSubjects.FindAll(x => x.Name == subject.Name);
                foreach (var match in matches)
                    matchedSubjectsCodes.Add(match.Code);
            }

            return matchedSubjectsCodes;
        }

    }
}
