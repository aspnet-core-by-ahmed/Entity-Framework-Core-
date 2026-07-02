namespace EF11.EntityFrameworkCoreMigration2.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string OfficeName { get; set; } = null!;
        public string OfficeLocation { get; set; } = null!;
        public Instructor? Instructor { get; set; }

    }
}