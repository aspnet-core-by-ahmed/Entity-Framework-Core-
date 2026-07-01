using EF10.EntityFrameworkCoreMigration1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF10.EntityFrameworkCoreMigration1.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // builder.Property(x => x.CourseName).HasMaxLength(255); // nvarchar(255)
            // builder.Property(x => x.CourseName).HasMaxLength(255).IsRequired(); // nvarchar(255) NOT NULL
            builder.Property(x => x.CourseName).HasColumnType("VARCHAR").HasMaxLength(255); // varchar 

            builder.Property(x => x.Price).HasPrecision(15, 2); // decimal(15,2) means 15 digits in total, with 2 digits after the decimal point

            builder.ToTable("Courses");
            // this is how you can specify the table name for the entity. By default, EF Core will use the class name as the table name, but you can override it using the ToTable method.

            // this is how you can seed data in EF Core using the Fluent API.
            // The HasData method is used to specify the initial data that should be inserted into the database when the migration is applied.
            //  In this case, we are seeding courses with their respective Id, CourseName, and Price values.
            builder.HasData(LoadCourses());
        }

        // i make it static so that it can be called without creating an instance of the class. 
        // This is useful for seeding data, as we don't need to create an object of CourseConfiguration just to get the list of courses.
        // i make it private because it is only used within this class and should not be accessible from outside.
        // non-static can call static methods, but static methods cannot call non-static methods because they do not have an instance of the class to work with.
        private static List<Course> LoadCourses()
        {
            List<Course> courses = new List<Course>();
            courses.Add(new Course() { Id = 1, CourseName = "Mathematics", Price = 1000 });
            courses.Add(new Course() { Id = 2, CourseName = "Physics", Price = 2000 });
            courses.Add(new Course() { Id = 3, CourseName = "Chemistry", Price = 1500 });
            courses.Add(new Course() { Id = 4, CourseName = "Biology", Price = 1200 });
            courses.Add(new Course() { Id = 5, CourseName = "CS-50", Price = 3000 });
            return courses;
        }
    }
}