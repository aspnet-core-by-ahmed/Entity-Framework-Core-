using EF11.EntityFrameworkCoreMigration2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF11.EntityFrameworkCoreMigration2.Data.Config
{
    public class SectionScheduleConfiguration : IEntityTypeConfiguration<SectionSchedule>
    {
        public void Configure(EntityTypeBuilder<SectionSchedule> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.StartTime).HasColumnType("time");
            builder.Property(x => x.EndTime).HasColumnType("time");

            builder.ToTable("SectionSchedules");

            builder.HasData(LoadSectionSchedulesData());
        }

        private static SectionSchedule[] LoadSectionSchedulesData()
        {
            return new[]
            {
                new SectionSchedule { Id = 1, SectionId = 1, ScheduleId = 1, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
                new SectionSchedule { Id = 2, SectionId = 2, ScheduleId = 3, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                new SectionSchedule { Id = 3, SectionId = 3, ScheduleId = 4, StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(1, 0, 0) },
                new SectionSchedule { Id = 4, SectionId = 4, ScheduleId = 1, StartTime = new TimeSpan(2, 0, 0), EndTime = new TimeSpan(4, 0, 0) },
                new SectionSchedule { Id = 5, SectionId = 5, ScheduleId = 1, StartTime = new TimeSpan(3, 0, 0), EndTime = new TimeSpan(5, 0, 0) },
                new SectionSchedule { Id = 6, SectionId = 6, ScheduleId = 2, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                new SectionSchedule { Id = 7, SectionId = 7, ScheduleId = 3, StartTime = new TimeSpan(1, 0, 0), EndTime = new TimeSpan(3, 0, 0) },
                new SectionSchedule { Id = 8, SectionId = 8, ScheduleId = 4, StartTime = new TimeSpan(2, 0, 0), EndTime = new TimeSpan(4, 0, 0) },
                new SectionSchedule { Id = 9, SectionId = 9, ScheduleId = 4, StartTime = new TimeSpan(3, 0, 0), EndTime = new TimeSpan(5, 0, 0) },
                new SectionSchedule { Id = 10, SectionId = 10, ScheduleId = 3, StartTime = new TimeSpan(4, 0, 0), EndTime = new TimeSpan(6, 0, 0) },
                new SectionSchedule { Id = 11, SectionId = 11, ScheduleId = 5, StartTime = new TimeSpan(5, 0, 0), EndTime = new TimeSpan(7, 0, 0) }
            };
        }
    }

}