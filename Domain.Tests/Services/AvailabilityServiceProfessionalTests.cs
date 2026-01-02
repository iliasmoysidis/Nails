using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;
using Fakes;
using FluentAssertions;

namespace Services;

public class AvailabilityServiceProfessionalTests
{
    private static StaffCalendar SetCalendar()
    {
        var calendar = StaffCalendar.Create(1, 1);
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
    public async Task EnsureProfessionalIsAvailableAsync_ShouldNotThrow_WhenProfessionalIsAvailable()
    {
        var calendar = SetCalendar();

        var storeRepo = new FakeStoreCalendarRepository(StoreCalendar.Create(1));
        var staffRepo = new FakeStaffCalendarRepository(calendar);

        var service = new AvailabilityService(storeRepo, staffRepo);

        var start = UtcDateTime.From(
            new DateTime(2025, 12, 22, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(1);

        await service.Invoking(s => s.EnsureProfessionalIsAvailableAsync(1, 1, start, end)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureProfessionalIsAvailableAsync_ShouldThrow_WhenProfessionalUnavailable()
    {
        var calendar = SetCalendar();

        var storeRepo = new FakeStoreCalendarRepository(StoreCalendar.Create(1));
        var staffRepo = new FakeStaffCalendarRepository(calendar);

        var service = new AvailabilityService(storeRepo, staffRepo);

        var start = UtcDateTime.From(
            new DateTime(2025, 12, 28, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(1);

        await service.Invoking(s => s.EnsureProfessionalIsAvailableAsync(1, 1, start, end)).Should().ThrowAsync<DomainException>().WithMessage("Professional is unavailable at the requested time.");
    }
}