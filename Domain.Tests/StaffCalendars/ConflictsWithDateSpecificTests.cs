using Domain.Entities;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace StaffCalendars;

public class ConflictsWithDateSpecificTests
{
    [Fact]
    public void ConflictsWithDateSpecific_ShouldReturnTrue_WhenExceptionOverlapsWithWorkingRanges()
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

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 17),
            new[]
            {
                new TimeRange(TimeSpan.FromHours(10), TimeSpan.FromHours(16))
            }
        );

        calendar.ConflictsWithDateSpecific(exception).Should().Be(true);
    }

    [Fact]
    public void ConflictsWithDateSpecific_ShouldReturnFalse_WhenExceptionIsDayOff()
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

        var exception = CalendarException.DayOff(
            new DateOnly(2025, 11, 17)
        );

        calendar.ConflictsWithDateSpecific(exception).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithDateSpecific_ShouldReturnFalse_WhenNoWorkDays()
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

        var exception = CalendarException.DayOff(
            new DateOnly(2025, 11, 17)
        );

        calendar.ConflictsWithDateSpecific(exception).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithDateSpecific_ShouldReturnFalse_WhenNoOverlap()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(14))
                }
            )
        );

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 17),
            new[]
            {
                new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(21))
            }
        );

        calendar.ConflictsWithDateSpecific(exception).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithDateSpecific_ShouldReturnFalse_WhenNoWorkingRangesExist()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 17),
            new[]
            {
                new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(21))
            }
        );

        calendar.ConflictsWithDateSpecific(exception).Should().Be(false);
    }
}