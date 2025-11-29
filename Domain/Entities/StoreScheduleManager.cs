using Domain.Exceptions;

namespace Domain.Entities;

public class StoreScheduleManager
{
    public int StoreId { get; private set; }

    private readonly List<StoreSchedule> _schedules = new();
    public IReadOnlyCollection<StoreSchedule> Schedules => _schedules.AsReadOnly();

    private readonly List<StoreScheduleSpecial> _exceptions = new();
    public IReadOnlyCollection<StoreScheduleSpecial> Exceptions => _exceptions.AsReadOnly();

    private StoreScheduleManager() { }

    public static StoreScheduleManager Create(int storeId)
    {
        return new StoreScheduleManager
        {
            StoreId = storeId
        };
    }

    public StoreSchedule AddStoreSchedule(DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        var schedule = StoreSchedule.Create(StoreId, day, openTime, closeTime);

        if (schedule.IsFullDayClosed)
        {
            bool isPartialOpen = _schedules.Any(e => e.Day == day && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial schedule exists for the day.");
            }
        }
        else
        {
            bool isFullClosedDay = _schedules.Any(e => e.Day == day && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _schedules.Any(
                e => e.Day == day &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                e.OpenTime.Value < closeTime &&
                e.CloseTime.Value > openTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Operating hours overlap with an existing schedule for this day.");
            }
        }

        _schedules.Add(schedule);
        return schedule;
    }

    public void RemoveStoreSchedule(int scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Schedule not found.");
        }

        _schedules.Remove(schedule);
    }

    public StoreScheduleSpecial AddStoreScheduleSpecial(DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        var exception = StoreScheduleSpecial.Create(StoreId, date, openTime, closeTime, reason);

        if (exception.IsFullDayClosed)
        {
            bool isPartialOpen = _exceptions.Any(e => e.Date.Date == date.Date && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial exceptions exist for the day.");
            }
        }
        else
        {
            bool isFullClosedDay = _exceptions.Any(e => e.Date.Date == date.Date && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _exceptions.Any(
                e => e.Date.Date == date.Date &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                e.OpenTime.Value < closeTime &&
                e.CloseTime.Value > openTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Exception overlaps with an existing partial block for this day.");
            }
        }

        _exceptions.Add(exception);
        return exception;
    }

    public void RemoveStoreScheduleSpecial(int exceptionId)
    {
        var exception = _exceptions.FirstOrDefault(e => e.Id == exceptionId);

        if (exception == null)
        {
            throw new DomainException("Exception not found.");
        }

        _exceptions.Remove(exception);
    }

    public bool IsOpenAt(DateTime date)
    {
        var exceptions = _exceptions.Where(e => e.Date.Date == date.Date).ToList();

        if (exceptions.Any())
        {
            return exceptions.Any(
                e => e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                date.TimeOfDay >= e.OpenTime.Value &&
                date.TimeOfDay < e.CloseTime.Value
            );
        }

        var schedules = _schedules.Where(h => h.Day == date.DayOfWeek).ToList();

        return schedules.Any(
            h => h.OpenTime.HasValue &&
            h.CloseTime.HasValue &&
            date.TimeOfDay >= h.OpenTime.Value &&
            date.TimeOfDay < h.CloseTime.Value);
    }

    public bool IsOpenOn(DayOfWeek day)
    {
        return !_schedules.Any(h => h.Day == day && h.IsFullDayClosed);
    }

    public bool IsWithinStoreHours(DayOfWeek day, TimeSpan? startTime, TimeSpan? endTime)
    {
        return _schedules.Any(h => h.Day == day && h.OpenTime <= startTime && h.CloseTime >= endTime);
    }
}