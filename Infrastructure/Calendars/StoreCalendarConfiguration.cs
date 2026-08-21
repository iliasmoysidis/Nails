using Domain.Calendars;
using Domain.Common.ValueObjects.Calendars;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Calendars;

public sealed class StoreCalendarConfiguration : IEntityTypeConfiguration<StoreCalendar>
{
    public void Configure(EntityTypeBuilder<StoreCalendar> builder)
    {
        builder.ToTable("StoreCalendars");
        builder.HasKey(x => x.StoreId);
        builder.Property(x => x.StoreId)
            .ValueGeneratedNever();

        builder.HasOne<Store>()
            .WithOne()
            .HasForeignKey<StoreCalendar>(x => x.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany<WorkingDay>("StoreCalendarWorkingDay",x => x.WorkingDays, workingDays =>
        {
            workingDays.ToTable("StoreCalendarWorkingDays");
            workingDays.WithOwner()
                .HasForeignKey("StoreId");

            workingDays.Property(x => x.Day)
                .HasConversion<int>()
                .IsRequired();

            workingDays.HasKey("StoreId", nameof(WorkingDay.Day));

            workingDays.OwnsMany<TimeRange>("StoreCalendarWorkingDayTimeRange", x => x.TimeRanges, ranges =>
            {
                ranges.ToTable("StoreCalendarWorkingDayTimeRanges");

                ranges.WithOwner()
                    .HasForeignKey("StoreId", "Day");

                ranges.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                ranges.HasKey("Id");

                ranges.Property(x => x.Start)
                    .HasColumnName("Start")
                    .IsRequired();

                ranges.Property(x => x.End)
                    .HasColumnName("End")
                    .IsRequired();
            });
        });

        builder.OwnsMany<CalendarException>("StoreCalendarException", x => x.Exceptions, exceptions =>
        {
            exceptions.ToTable("StoreCalendarExceptions");

            exceptions.WithOwner()
                .HasForeignKey("StoreId");

            exceptions.Property(x => x.Date)
                .IsRequired();

            exceptions.HasKey("StoreId", nameof(CalendarException.Date));

            exceptions.OwnsMany<TimeRange>("StoreCalendarExceptionTimeRanges", x => x.TimeRanges, ranges =>
            {
                ranges.ToTable("StoreExceptionTimeRanges");

                ranges.WithOwner()
                    .HasForeignKey("StoreId", "Date");

                ranges.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                ranges.HasKey("Id");

                ranges.Property(x => x.Start)
                    .HasColumnName("Start")
                    .IsRequired();

                ranges.Property(x => x.End)
                    .HasColumnName("End")
                    .IsRequired();
            });
        });

        builder.Navigation(x => x.WorkingDays)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.Exceptions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
