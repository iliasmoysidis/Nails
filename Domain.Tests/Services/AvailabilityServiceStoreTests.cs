using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;
using Domain.Tests.Fakes;
using FluentAssertions;

namespace Services;

public class AvailabilityServiceStoreTests
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
    public async Task EnsureStoreIsOpenAsync_ShouldNotThrow_WhenStoreIsOpen()
    {
        var calendar = SetCalendar();

        var storeRepo = new FakeStoreCalendarRepository(calendar);
        var staffRepo = new FakeStaffCalendarRepository(StaffCalendar.Create(1, 1));

        var service = new AvailabilityService(storeRepo, staffRepo);

        var start = UtcDateTime.From(
            new DateTime(2025, 12, 22, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(1);

        await service.Invoking(s => s.EnsureStoreIsOpenAsync(1, start, end)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureStoreIsOpenAsync_ShouldThrow_WhenStoreIsClosed()
    {
        var calendar = SetCalendar();

        var storeRepo = new FakeStoreCalendarRepository(calendar);
        var staffRepo = new FakeStaffCalendarRepository(StaffCalendar.Create(1, 1));

        var service = new AvailabilityService(storeRepo, staffRepo);

        var start = UtcDateTime.From(
            new DateTime(2025, 12, 31, 10, 0, 0, DateTimeKind.Utc)
        );

        var end = start.AddHours(1);

        await service.Invoking(s => s.EnsureStoreIsOpenAsync(1, start, end)).Should().ThrowAsync<DomainException>().WithMessage("Store is closed at the requested time.");
    }
}