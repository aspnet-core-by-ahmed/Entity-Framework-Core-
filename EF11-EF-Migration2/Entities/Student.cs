namespace EF11.EntityFrameworkCoreMigration2.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;

        public ICollection<Section> Sections = new List<Section>();
        public ICollection<Enrollment> Enrollments = new List<Enrollment>();


    }
}