using Domain.Entities;
using Domain.ValueObjects.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StaffCalendarConfig : IEntityTypeConfiguration<StaffCalendar>
{
    public void Configure(EntityTypeBuilder<StaffCalendar> builder)
    {
        builder.ToTable("staff_calendars");

        builder.HasKey(c => new { c.StoreId, c.ProfessionalId });

        builder.Property(c => c.StoreId)
            .ValueGeneratedNever();

        builder.Property(c => c.ProfessionalId)
            .ValueGeneratedNever();

        builder.OwnsMany<WorkingDay>("_workingDays", wd =>
        {
            wd.ToTable("staff_working_days");

            wd.WithOwner()
                .HasForeignKey("StoreId", "ProfessionalId");

            wd.Property<int>("StoreId");
            wd.Property<int>("ProfessionalId");

            wd.Property(w => w.Day)
                .HasConversion<int>()
                .IsRequired();

            wd.HasKey("StoreId", "ProfessionalId", nameof(WorkingDay.Day));

            wd.OwnsMany(w => w.TimeRanges, tr =>
            {
                tr.ToTable("staff_working_day_ranges");

                tr.WithOwner()
                    .HasForeignKey("StoreId", "ProfessionalId", "Day");

                tr.Property<int>("StoreId");
                tr.Property<int>("ProfessionalId");
                tr.Property<int>("Day");

                tr.Property(r => r.Start)
                    .HasColumnType("time")
                    .IsRequired();

                tr.Property(r => r.End)
                    .HasColumnType("time")
                    .IsRequired();

                tr.HasKey("StoreId", "ProfessionalId", "Day", "Start", "End");
            });
        });

        builder.Navigation("_workingDays")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany<CalendarException>("_exceptions", ex =>
        {
            ex.ToTable("staff_calendar_exceptions");

            ex.WithOwner()
                .HasForeignKey("StoreId", "ProfessionalId");

            ex.Property<int>("StoreId");
            ex.Property<int>("ProfessionalId");

            ex.Property(e => e.Date)
                .HasColumnType("date")
                .IsRequired();

            ex.HasKey("StoreId", "ProfessionalId", nameof(CalendarException.Date));

            ex.OwnsMany(e => e.TimeRanges, tr =>
            {
                tr.ToTable("staff_exception_ranges");

                tr.WithOwner()
                    .HasForeignKey("StoreId", "ProfessionalId", "Date");

                tr.Property<int>("StoreId");
                tr.Property<int>("ProfessionalId");
                tr.Property<DateOnly>("Date");

                tr.Property(r => r.Start)
                    .HasColumnType("time")
                    .IsRequired();

                tr.Property(r => r.End)
                    .HasColumnType("time")
                    .IsRequired();

                tr.HasKey("StoreId", "ProfessionalId", "Date", "Start", "End");
            });
        });

        builder.Navigation("_exceptions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}