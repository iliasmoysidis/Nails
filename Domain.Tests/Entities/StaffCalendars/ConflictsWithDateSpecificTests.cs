using Domain.Entities;
using Domain.ValueObjects.Calendar;
using FluentAssertions;

namespace Domain.Tests.Entities.StaffCalendars;

public class ConflictsWithDateSpecificTests
{
    private static StaffCalendar CreateCalendar()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                [Range(9, 17)]
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Tuesday,
                [Range(9, 17)]
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Wednesday,
                [Range(9, 17)]
            )
        );

        calendar.SetDayOff(DayOfWeek.Sunday);

        calendar.AddException(
            CalendarException.PartialDay(
                new DateOnly(2025, 1, 9),
                [Range(17, 21)]
            )
        );

        calendar.AddException(
            CalendarException.DayOff(
                new DateOnly(2025, 1, 20)
            )
        );

        return calendar;
    }

    private static TimeRange Range(int startHour, int endHour)
        => new(TimeSpan.FromHours(startHour), TimeSpan.FromHours(endHour));

    [Fact]
    public void ConflictsWithDateSpecific_ReturnsTrue_WhenExceptionOverlapsWorkingHours()
    {
        var calendar = CreateCalendar();

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 17),
            [Range(10, 16)]
        );

        calendar.ConflictsWithDateSpecific(exception).Should().BeTrue();
    }

    [Fact]
    public void ConflictsWithDateSpecific_ReturnsFalse_WhenExceptionIsDayOff()
    {
        var calendar = CreateCalendar();

        var exception = CalendarException.DayOff(
            new DateOnly(2025, 11, 17)
        );

        calendar.ConflictsWithDateSpecific(exception).Should().BeFalse();
    }

    [Fact]
    public void ConflictsWithDateSpecific_ReturnsFalse_WhenNoWorkingDayConfiguredForDate()
    {
        var calendar = CreateCalendar();

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 22),
            [Range(10, 12)]
        );

        calendar.ConflictsWithDateSpecific(exception).Should().BeFalse();
    }

    [Fact]
    public void ConflictsWithDateSpecific_ReturnsFalse_WhenNoOverlapWithWorkingHours()
    {
        var calendar = CreateCalendar();

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 11, 17),
            [Range(18, 21)]
        );

        calendar.ConflictsWithDateSpecific(exception).Should().BeFalse();
    }

    [Fact]
    public void ConflictsWithDateSpecific_ReturnsFalse_WhenCalendarHasNoWorkingRanges()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        var exception = CalendarException.PartialDay(
            new DateOnly(2025, 1, 20),
            [Range(15, 21)]
        );

        calendar.ConflictsWithDateSpecific(exception).Should().BeFalse();
    }
}
