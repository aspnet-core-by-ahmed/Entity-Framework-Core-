using EF11.EntityFrameworkCoreMigration2.Data.Config;
using EF11.EntityFrameworkCoreMigration2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF11.EntityFrameworkCoreMigration2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SectionSchedule> SectionSchedules { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var constr = config.GetSection("constr").Value;
            optionsBuilder.UseSqlServer(constr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // way 1
            // modelBuilder.ApplyConfiguration(new CourseConfiguration());
            // modelBuilder.ApplyConfiguration(new InstructorConfiguration());

            // way 2
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly);
        }
    }
}