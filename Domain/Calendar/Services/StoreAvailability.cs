using Domain.Common.Exceptions;
using Domain.Common.ValueObjects.Calendar;
using Domain.Stores;

namespace Domain.Calendar.Services;

public sealed class StoreAvailability
{
    private readonly Store _store;
    private readonly StoreCalendar _calendar;

    public StoreAvailability(Store store, StoreCalendar calendar)
    {
        ValidateComposition(store, calendar);

        _store = store;
        _calendar = calendar;
    }

    public void AddHoliday(DateOnly date)
    {
        _store.EnsureOpen();
        _calendar.AddHoliday(date);
    }

    public void SetSpecialOpeningHours(DateOnly date, IEnumerable<TimeRange> ranges)
    {
        _store.EnsureOpen();
        _calendar.SetSpecialOpeningHours(date, ranges);
    }

    public void RemoveSpecialOpeningHours(DateOnly date)
    {
        _store.EnsureOpen();
        _calendar.RemoveSpecialOpeningHours(date);
    }

    public void SetOpeningHours(DayOfWeek day, IEnumerable<TimeRange> ranges)
    {
        _store.EnsureOpen();
        _calendar.SetOpeningHours(day, ranges);
    }

    public void CloseDay(DayOfWeek day)
    {
        _store.EnsureOpen();
        _calendar.CloseDay(day);
    }

    private static void ValidateComposition(Store store, StoreCalendar calendar)
    {
        if (store.Id != calendar.StoreId)
            throw new InvariantException("Calendar does not belong to store.");
    }
}
