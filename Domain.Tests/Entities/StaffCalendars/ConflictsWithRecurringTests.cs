using Domain.Entities;
using Domain.ValueObjects.Calendar;
using FluentAssertions;

namespace Entities.StaffCalendars;

public class ConflictsWithRecurringTests
{
    [Fact]
    public void ConflictsWithRecurring_ShouldReturnTrue_WhenTimeRangeOverlaps()
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

        var incoming = WorkingDay.DayOff(DayOfWeek.Monday);

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnFalse_WhenThereIsNoOverlap()
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

        var incoming = WorkingDay.WithRanges(
                DayOfWeek.Tuesday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(12), TimeSpan.FromHours(15))
                }
            );

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }

    [Fact]
    public void ConflictsWithRecurring_ShouldReturnFalse_WhenItOverlapsWithDayOff()
    {
        var calendar = StaffCalendar.Create(storeId: 1, professionalId: 1);

        calendar.SetWorkingDay(
            WorkingDay.DayOff(DayOfWeek.Monday)
        );

        var incoming = WorkingDay.WithRanges(
                DayOfWeek.Monday,
                new[]
                {
                    new TimeRange(TimeSpan.FromHours(12), TimeSpan.FromHours(15))
                }
            );

        calendar.ConflictsWithRecurring(incoming).Should().Be(false);
    }
}