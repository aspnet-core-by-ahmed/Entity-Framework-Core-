namespace EF11.EntityFrameworkCoreMigration2.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;
        public decimal Price { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }

}