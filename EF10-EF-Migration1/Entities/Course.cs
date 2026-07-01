namespace EF10.EntityFrameworkCoreMigration1.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;
        public decimal Price { get; set; }
    }

}