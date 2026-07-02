namespace EF11.EntityFrameworkCoreMigration2.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;

        public int? OfficeId { get; set; }
        public Office? Office { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}