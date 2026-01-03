using Domain.Entities;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Entities.StoreCalendars;

public class IsOpenAtTests
{
    private static StoreCalendar MondayNineToFive()
    {
        var calendar = StoreCalendar.Create(1);
        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                [new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))]
            )
        );
        return calendar;
    }

    [Fact]
    public void IsOpenAt_ShouldReturnTrue_WhenInsideWorkingHours()
    {
        var calendar = MondayNineToFive();

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 10, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(true);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnTrue_WhenInsidePartialException()
    {
        var calendar = MondayNineToFive();

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 22),
            [
                new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(17))
            ]
            ));

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 16, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(true);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnTrue_AtWorkingRangeStart()
    {
        var calendar = MondayNineToFive();

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 9, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(true);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_AtWorkingRangeEnd()
    {
        var calendar = MondayNineToFive();

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 17, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_WhenExceptionIsDayOff()
    {
        var calendar = MondayNineToFive();

        calendar.AddException(CalendarException.DayOff(new DateOnly(2025, 12, 22)));

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 10, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_WhenOutsidePartialException()
    {
        var calendar = MondayNineToFive();

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 22),
            [
                new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(17))
            ]
            ));

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 21, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_WhenNoWorkingDayDefinedForDate()
    {
        var calendar = MondayNineToFive();

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 22),
            [
                new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(17))
            ]
            ));

        var time = UtcDateTime.From(new DateTime(2025, 12, 23, 21, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_WhenDayOff()
    {
        var calendar = StoreCalendar.Create(storeId: 1);

        calendar.SetWorkingDay(
            WorkingDay.DayOff(
                DayOfWeek.Monday
            )
        );

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 10, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }

    [Fact]
    public void IsOpenAt_ShouldReturnFalse_WhenOutsideWorkingHours()
    {
        var calendar = StoreCalendar.Create(storeId: 1);

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                [
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(12)),
                    new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))
                ]
            )
        );

        var time = UtcDateTime.From(new DateTime(2025, 12, 22, 14, 0, 0, DateTimeKind.Utc));

        calendar.IsOpenAt(time).Should().Be(false);
    }
}