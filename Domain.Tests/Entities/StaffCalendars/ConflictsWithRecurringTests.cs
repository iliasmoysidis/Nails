using Domain.Entities;
using Domain.ValueObjects.Calendar;
using FluentAssertions;

namespace Domain.Tests.Entities.StaffCalendars;

public class ConflictsWithRecurringTests
{
    private static StaffCalendar CreateCalendar()
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

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Tuesday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        calendar.SetWorkingDay(
            WorkingDay.WithRanges(
                DayOfWeek.Wednesday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(9), TimeSpan.FromHours(17))
                }
            )
        );

        calendar.AddException(
            CalendarException.PartialDay(
                new DateOnly(2025, 1, 9),
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(17), TimeSpan.FromHours(21))

                }
            )
        );

        calendar.AddException(CalendarException.DayOff(
            new DateOnly(2025, 1, 20)
            )
        );

        calendar.SetDayOff(DayOfWeek.Sunday);

        return calendar;
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnTrue_WhenTimeRangeOverlaps()
    {
        var calendar = CreateCalendar();

        var incoming = WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(12), TimeSpan.FromHours(15))
                }
            );

        calendar.ConflictsWithRecurring(incoming).Should().Be(true);
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnFalse_WhenDayOff()
    {
        var calendar = CreateCalendar();

        var incoming = WorkingDay.DayOff(DayOfWeek.Monday);

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnFalse_WhenThereIsNoOverlap()
    {
        var calendar = CreateCalendar();

        var incoming = WorkingDay.WithRanges(
                DayOfWeek.Tuesday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(18), TimeSpan.FromHours(20))
                }
            );

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnFalse_WhenItOverlapsWithDayOff()
    {
        var calendar = CreateCalendar();

        var incoming = WorkingDay.WithRanges(
                DayOfWeek.Sunday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(12), TimeSpan.FromHours(15))
                }
            );

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }
}