using Domain.Common.ValueObjects.Calendar;
using Domain.Professionals;
using Domain.Schedule;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Schedule;

public sealed class ProfessionalScheduleConfiguration : IEntityTypeConfiguration<ProfessionalSchedule>
{
    public void Configure(EntityTypeBuilder<ProfessionalSchedule> builder)
    {
        builder.ToTable("ProfessionalSchedules");

        builder.HasKey(x => x.ProfessionalId);

        builder.Property(x => x.ProfessionalId)
            .ValueGeneratedNever();

        builder.HasOne<Professional>()
            .WithOne()
            .HasForeignKey<ProfessionalSchedule>(x => x.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(x => x.Calendars, calendars =>
        {
            calendars.ToTable("StaffCalendars");

            calendars.WithOwner()
                .HasForeignKey("ProfessionalId");

            calendars.HasKey(
                x => new
                {
                    x.ProfessionalId,
                    x.StoreId
                }
            );

            calendars.Property(x => x.StoreId)
                .ValueGeneratedNever();

            calendars.HasOne<Store>()
                .WithMany()
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            calendars.OwnsMany(x => x.WorkingDays, workingDays =>
            {
                workingDays.ToTable("StaffCalendarWorkingDays");

                workingDays.WithOwner()
                    .HasForeignKey("ProfessionalId", "StoreId");

                workingDays.HasKey("ProfessionalId", "StoreId", nameof(WorkingDay.Day));

                workingDays.Property(x => x.Day)
                    .HasConversion<int>()
                    .IsRequired();

                workingDays.OwnsMany(x => x.TimeRanges, ranges =>
                {
                    ranges.ToTable("StaffCalendarWorkingDayTimeRanges");

                    ranges.WithOwner()
                        .HasForeignKey("ProfessionalId", "StoreId", "Day");

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

            calendars.OwnsMany(x => x.Exceptions, exceptions =>
            {
                exceptions.ToTable("StaffCalendarExceptions");

                exceptions.WithOwner()
                    .HasForeignKey("ProfessionalId", "StoreId");

                exceptions.Property(x => x.Date)
                    .IsRequired();

                exceptions.HasKey("ProfessionalId", "StoreId", nameof(CalendarException.Date));

                exceptions.OwnsMany(x => x.TimeRanges, ranges =>
                {
                    ranges.ToTable("StaffCalendarExceptionTimeRanges");

                    ranges.WithOwner()
                        .HasForeignKey("ProfessionalId", "StoreId", "Date");

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

            calendars.Navigation(x => x.WorkingDays)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            calendars.Navigation(x => x.Exceptions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Navigation(x => x.Calendars)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
