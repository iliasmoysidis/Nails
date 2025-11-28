using Domain.Exceptions;

namespace Domain.Entities;

public class StoreStaffScheduleManager
{
    public int StoreId { get; private set; }

    private readonly List<StoreStaffSchedule> _schedules = new();
    public IReadOnlyCollection<StoreStaffSchedule> Schedules => _schedules.AsReadOnly();

    private readonly List<StoreStaffScheduleSpecial> _exceptions = new();
    public IReadOnlyCollection<StoreStaffScheduleSpecial> Exceptions => _exceptions.AsReadOnly();

    private StoreStaffScheduleManager() { }

    public StoreStaffScheduleManager Create(int storeId)
    {
        return new StoreStaffScheduleManager
        {
            StoreId = storeId,
        };
    }

    public StoreStaffSchedule AddStaffSchedule(int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        var schedule = StoreStaffSchedule.Create(StoreId, professionalId, day, startTime, endTime);

        if (schedule.IsPTO)
        {
            bool isWorking = _schedules.Any(s => s.ProfessionalId == professionalId && s.Day == day && s.IsWorking);

            if (isWorking)
            {
                throw new DomainException("Cannot create full-day off when partial schedule exists.");
            }
        }
        else
        {
            bool isOverlapping = _schedules.Any(
                s => s.ProfessionalId == professionalId &&
                s.Day == day &&
                s.StartTime.HasValue &&
                s.EndTime.HasValue &&
                s.StartTime.Value < endTime &&
                s.EndTime.Value > startTime);

            if (isOverlapping)
            {
                throw new DomainException("Schedule overlaps with existing schedule.");
            }
        }

        _schedules.Add(schedule);
        return schedule;
    }

    public void RemoveStaffSchedule(int scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Could not find schedule.");
        }

        _schedules.Remove(schedule);
    }

    public StoreStaffScheduleSpecial AddStaffException(int professionalId, DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {

        var exception = StoreStaffScheduleSpecial.Create(StoreId, professionalId, date, startTime, endTime, reason);

        if (exception.IsDayOff)
        {
            bool isPartiallyWorking = _exceptions.Any(
                e => e.ProfessionalId == professionalId &&
                e.Date == date &&
                !e.IsDayOff
            );

            if (isPartiallyWorking)
            {
                throw new DomainException("Cannot schedule staff to partially work on a day off.");
            }
        }
        else
        {
            bool isOverlapping = _exceptions.Any(
                e => e.ProfessionalId == professionalId &&
                e.Date == date &&
                e.StartTime.HasValue &&
                e.EndTime.HasValue &&
                e.StartTime.Value < endTime &&
                e.EndTime.Value > startTime
                );

            if (isOverlapping)
            {
                throw new DomainException("Exception overlaps with an existing exception for this professional.");
            }
        }

        _exceptions.Add(exception);
        return exception;
    }

    public void RemoveStaffException(int exceptionId)
    {
        var exception = _exceptions.FirstOrDefault(e => e.Id == exceptionId);

        if (exception == null)
        {
            throw new DomainException("Exception not found.");
        }

        _exceptions.Remove(exception);
    }

    public bool IsProfessionalAvailable(int professionalId, DateTime startAt, DateTime endAt)
    {
        var day = startAt.DayOfWeek;

        var daySchedules = _schedules.Where(s => s.ProfessionalId == professionalId && s.Day == day && s.IsWorking).ToList();

        if (!daySchedules.Any())
        {
            return false;
        }

        bool isWithinSchedule = daySchedules.Any(s => startAt.TimeOfDay >= s.StartTime!.Value && endAt.TimeOfDay <= s.EndTime!.Value);
        if (!isWithinSchedule)
        {
            return false;
        }

        var dateExceptions = _exceptions.Where(e => e.ProfessionalId == professionalId && e.Date.Date == startAt.Date).ToList();

        foreach (var exception in dateExceptions)
        {
            if (exception.IsBlocking(startAt, endAt))
            {
                return false;
            }
        }

        return true;
    }
}