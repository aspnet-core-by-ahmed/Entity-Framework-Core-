namespace EF11.EntityFrameworkCoreMigration2.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = null!;


        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;


        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }


        public ICollection<Schedule> Schedules = new List<Schedule>();
        public ICollection<SectionSchedule> SectionSchedules = new List<SectionSchedule>();


        public ICollection<Student> Students = new List<Student>();
        public ICollection<Enrollment> Enrollments = new List<Enrollment>();



    }
}