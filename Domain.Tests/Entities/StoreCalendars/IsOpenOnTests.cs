using Domain.Entities;
using Domain.ValueObjects.Calendar;
using FluentAssertions;

namespace Domain.Tests.Entities.StoreCalendars;

public class IsOpenOnTests
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
    public void IsOpenOn_ShouldReturnTrue_WhenOnWorkingDay()
    {
        var calendar = MondayNineToFive();

        var date = new DateOnly(2025, 12, 29);

        calendar.IsOpenOn(date).Should().Be(true);
    }

    [Fact]
    public void IsOpenOn_ShouldReturnTrue_WhenOnPartialWorkingDay()
    {
        var calendar = MondayNineToFive();
        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 29),
            [new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))]
        ));

        var date = new DateOnly(2025, 12, 29);

        calendar.IsOpenOn(date).Should().Be(true);
    }

    [Fact]
    public void IsOpenOn_ShouldReturnFalse_WhenOnDayOff()
    {
        var calendar = MondayNineToFive();
        calendar.AddException(CalendarException.DayOff(
            new DateOnly(2025, 12, 29)
        ));

        var date = new DateOnly(2025, 12, 29);

        calendar.IsOpenOn(date).Should().Be(false);
    }

    [Fact]
    public void IsOpenOn_ShouldReturnFalse_WhenNoWorkingDayOrNoExceptionn()
    {
        var calendar = MondayNineToFive();
        calendar.AddException(CalendarException.PartialDay(
            new DateOnly(2025, 12, 30),
            [new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))]
        ));

        var date = new DateOnly(2025, 12, 31);

        calendar.IsOpenOn(date).Should().Be(false);
    }
}