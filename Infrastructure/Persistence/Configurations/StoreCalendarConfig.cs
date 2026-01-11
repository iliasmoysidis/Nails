using Domain.Entities;
using Domain.ValueObjects.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StoreCalendarConfig : IEntityTypeConfiguration<StoreCalendar>
{
    public void Configure(EntityTypeBuilder<StoreCalendar> builder)
    {
        builder.ToTable("store_calendars");

        builder.HasKey(c => c.StoreId);

        builder.Property(c => c.StoreId)
            .ValueGeneratedNever();

        builder.OwnsMany<WorkingDay>("_workingDays", wd =>
        {
            wd.ToTable("store_working_days");

            wd.WithOwner()
                .HasForeignKey("StoreId");

            wd.Property<int>("StoreId");
            wd.Property<int>("Day");

            wd.Property(w => w.Day)
                .HasConversion<int>()
                .IsRequired();

            wd.HasKey("StoreId", nameof(WorkingDay.Day));

            wd.OwnsMany(w => w.TimeRanges, tr =>
            {
                tr.ToTable("store_working_day_ranges");

                tr.WithOwner()
                    .HasForeignKey("StoreId", "Day");

                tr.Property<int>("StoreId");
                tr.Property<int>("Day");

                tr.Property(r => r.Start)
                    .HasColumnType("time")
                    .IsRequired();

                tr.Property(r => r.End)
                    .HasColumnType("time")
                    .IsRequired();

                tr.HasKey("StoreId", "Day", "Start", "End");
            });
        });

        builder.Navigation("_workingDays")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany<CalendarException>("_exceptions", ex =>
        {
            ex.ToTable("store_calendar_exceptions");

            ex.WithOwner()
                .HasForeignKey("StoreId");

            ex.Property<int>("StoreId");

            ex.Property(e => e.Date)
                .HasColumnType("date")
                .IsRequired();

            ex.HasKey("StoreId", nameof(CalendarException.Date));

            ex.OwnsMany(e => e.TimeRanges, tr =>
            {
                tr.ToTable("store_exception_ranges");

                tr.WithOwner()
                    .HasForeignKey("StoreId", "Date");

                tr.Property<int>("StoreId");
                tr.Property<DateOnly>("Date");

                tr.Property(r => r.Start)
                    .HasColumnType("time")
                    .IsRequired();

                tr.Property(r => r.End)
                    .HasColumnType("time")
                    .IsRequired();

                tr.HasKey("StoreId", "Date", "Start", "End");
            });
        });

        builder.Navigation("_exceptions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}