using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.StaffCalendars;

public class AvailabilityTests
{
    [Fact]
    public void IsProfessionalAvailable_ShouldReturnTrue_WhenInsideWorkingHours()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(1);

        calendar.IsProfessionalAvailable(start, end).Should().Be(true);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnTrue_WhenInsidePartialException()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 1, 6),
            new[]
            {
                new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))

            }
        )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 18, 0, 0, DateTimeKind.Utc)  // Monday
        );

        var end = start.AddHours(2);

        calendar.IsProfessionalAvailable(start, end).Should().Be(true);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnFalse_WhenOutsideWorkingHours()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(10);

        calendar.IsProfessionalAvailable(start, end).Should().Be(false);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnFalse_WhenDayOff()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetDayOff(DayOfWeek.Monday);

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)  // Monday
        );

        var end = start.AddHours(2);

        calendar.IsProfessionalAvailable(start, end).Should().Be(false);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnFalse_WhenOutsidePartialException()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 1, 6),
            new[]
            {
                new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))

            }
        )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)  // Monday
        );

        var end = start.AddHours(2);

        calendar.IsProfessionalAvailable(start, end).Should().Be(false);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnFalse_WhenExceptionDayOff()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.AddException(CalendarException.DayOff(
            new DateOnly(2025, 1, 6)
            )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)  // Monday
        );

        var end = start.AddHours(2);

        calendar.IsProfessionalAvailable(start, end).Should().Be(false);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldReturnFalse_WhenEndDateIsDifferentThanStartDate()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(24);

        calendar.IsProfessionalAvailable(start, end).Should().Be(false);
    }

    [Fact]
    public void IsProfessionalAvailable_ShouldThrow_WhenEndBeforeStartTime()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        var start = UtcDateTime.From(
            new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(-1);

        Action act = () => calendar.IsProfessionalAvailable(start, end);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("End time must be after start time.");
    }


}