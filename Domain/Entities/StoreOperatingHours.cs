using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreOperatingHours : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }

    private readonly List<StoreSchedule> _weeklySchedule = new();
    public IReadOnlyCollection<StoreSchedule> WeeklySchedule => _weeklySchedule.AsReadOnly();

    private readonly List<StoreException> _storeExceptions = new();
    public IReadOnlyCollection<StoreException> StoreExceptions => _storeExceptions.AsReadOnly();

    [Timestamp]
    public byte[] RowVersion { get; private set; } = null!;

    public static StoreOperatingHours Create(int storeId)
    {
        return new StoreOperatingHours
        {
            StoreId = storeId
        };
    }

    public StoreSchedule AddSchedule(DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        var schedule = StoreSchedule.Create(Id, day, openTime, closeTime);

        if (schedule.IsFullDayClosed)
        {
            bool isPartialOpen = _weeklySchedule.Any(e => e.Day == day && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial schedule exists for the day.");
            }
        }
        else
        {
            bool isFullClosedDay = _weeklySchedule.Any(e => e.Day == day && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _weeklySchedule.Any(
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

        _weeklySchedule.Add(schedule);
        MarkAsUpdated();

        return schedule;
    }

    public void RemoveSchedule(int scheduleId)
    {
        var schedule = _weeklySchedule.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Schedule not found.");
        }

        _weeklySchedule.Remove(schedule);
        MarkAsUpdated();
    }

    public StoreException AddException(DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        var exception = StoreException.Create(Id, date, openTime, closeTime, reason);

        if (exception.IsFullDayClosed)
        {
            bool isPartialOpen = _storeExceptions.Any(e => e.Date.Date == date.Date && !e.IsFullDayClosed);

            if (isPartialOpen)
            {
                throw new DomainException("Cannot create a full-day closure when partial exceptions exist for the day");
            }
        }
        else
        {
            bool isFullClosedDay = _storeExceptions.Any(e => e.Date.Date == date.Date && e.IsFullDayClosed);

            if (isFullClosedDay)
            {
                throw new DomainException("Cannot create a partial block on a fully closed day.");
            }

            bool isOverlapping = _storeExceptions.Any(
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

        _storeExceptions.Add(exception);
        MarkAsUpdated();

        return exception;
    }

    public void RemoveStoreException(int exceptionId)
    {
        var exception = _storeExceptions.FirstOrDefault(e => e.Id == exceptionId);

        if (exception == null)
        {
            throw new DomainException("Exception not found.");
        }

        _storeExceptions.Remove(exception);
        MarkAsUpdated();
    }

    public bool IsOpenAt(DateTime date)
    {
        var dateExceptions = _storeExceptions.Where(e => e.Date.Date == date.Date).ToList();

        if (dateExceptions.Any())
        {
            return dateExceptions.Any(
                e => !e.IsFullDayClosed &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                date.TimeOfDay >= e.OpenTime.Value &&
                date.TimeOfDay < e.CloseTime.Value
            );
        }

        var dayHours = _weeklySchedule.Where(h => h.Day == date.DayOfWeek).ToList();

        return dayHours.Any(
            h => !h.IsFullDayClosed &&
            h.OpenTime.HasValue &&
            h.CloseTime.HasValue &&
            date.TimeOfDay >= h.OpenTime.Value &&
            date.TimeOfDay < h.CloseTime.Value);
    }

    public bool IsOpenOn(DayOfWeek day)
    {
        return !_weeklySchedule.Any(h => h.Day == day && h.IsFullDayClosed);
    }

    public bool IsWithinStoreHours(DayOfWeek day, TimeSpan? startTime, TimeSpan? endTime)
    {
        return _weeklySchedule.Any(h => h.Day == day && h.OpenTime <= startTime && h.CloseTime >= endTime);
    }
}