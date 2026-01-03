using Domain.Entities;
using Domain.ValueObjects.Calendar;

using FluentAssertions;

namespace Domain.Tests.Entities.StoreCalendars;

public class IsWithinStoreHoursTests
{
    private static StoreCalendar SetCalendar()
    {
        var calendar = StoreCalendar.Create(1);
        calendar.SetWorkingDay(
            WorkingDay.DayOff(
                DayOfWeek.Sunday
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Monday,
                [new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))]
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Tuesday,
                [new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))]
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Wednesday,
                [new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))]
            )
        );

        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 30),
            [new TimeRange(TimeSpan.FromHours(15), TimeSpan.FromHours(20))]
        ));

        calendar.AddException(CalendarException.DayOff(
            new DateOnly(2025, 12, 31)
        ));

        return calendar;
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnTrue_WhenInsideRegularWorkingHours()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 29);
        var range = new TimeRange(TimeSpan.FromHours(10), TimeSpan.FromHours(11));

        calendar.IsWithinStoreHours(date, range).Should().Be(true);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnTrue_WhenRangeMatchesWorkingHoursExactly()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 29);
        var range = new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17));

        calendar.IsWithinStoreHours(date, range).Should().Be(true);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnTrue_WhenInsidePartialDayWorkingHours()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 30);
        var range = new TimeRange(TimeSpan.FromHours(16), TimeSpan.FromHours(18));

        calendar.IsWithinStoreHours(date, range).Should().Be(true);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnFalse_WhenExceptionDayOff()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 31);
        var range = new TimeRange(TimeSpan.FromHours(13), TimeSpan.FromHours(15));

        calendar.IsWithinStoreHours(date, range).Should().Be(false);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnFalse_WhenWorkingDayIsOff()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 28);
        var range = new TimeRange(TimeSpan.FromHours(13), TimeSpan.FromHours(15));

        calendar.IsWithinStoreHours(date, range).Should().Be(false);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnFalse_WhenNotInsideWorkingHours()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 29);
        var range = new TimeRange(TimeSpan.FromHours(13), TimeSpan.FromHours(18));

        calendar.IsWithinStoreHours(date, range).Should().Be(false);
    }

    [Fact]
    public void IsWithinStoreHours_ShouldReturnFalse_WhenNotInsidePartialWorkingHours()
    {
        var calendar = SetCalendar();

        var date = new DateOnly(2025, 12, 30);
        var range = new TimeRange(TimeSpan.FromHours(14), TimeSpan.FromHours(18));

        calendar.IsWithinStoreHours(date, range).Should().Be(false);
    }
}