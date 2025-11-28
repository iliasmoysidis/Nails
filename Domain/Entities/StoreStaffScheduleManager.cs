using Domain.Exceptions;

namespace Domain.Entities;

public class StoreStaffScheduleManager
{
    public int Id { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }

    private readonly List<StoreSchedule> _storeSchedules = new();
    public IReadOnlyCollection<StoreSchedule> StoreSchedules => _storeSchedules.AsReadOnly();

    private readonly List<StoreProfessionalSchedule> _weeklySchedule = new();
    public IReadOnlyCollection<StoreProfessionalSchedule> WeeklySchedule => _weeklySchedule.AsReadOnly();

    private readonly List<StoreProfessionalException> _professionalExceptions = new();
    public IReadOnlyCollection<StoreProfessionalException> ProfessionalExceptions => _professionalExceptions.AsReadOnly();

    private StoreStaffScheduleManager() { }

    public StoreProfessionalSchedule AddStaffSchedule(DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        var schedule = StoreProfessionalSchedule.Create(StoreId, ProfessionalId, day, startTime, endTime);

        if (schedule.IsPTO)
        {
            bool isWorking = _weeklySchedule.Any(s => s.Day == day && s.IsWorking);

            if (isWorking)
            {
                throw new DomainException("Cannot create full-day off when partial schedule exists.");
            }
        }
        else
        {
            bool isOverlapping = _weeklySchedule.Any(
                s => s.Day == day &&
                s.StartTime.HasValue &&
                s.EndTime.HasValue &&
                s.StartTime.Value < endTime &&
                s.EndTime.Value > startTime);

            if (isOverlapping)
            {
                throw new DomainException("Schedule overlaps with existing schedule.");
            }
        }

        _weeklySchedule.Add(schedule);
        return schedule;
    }

    public void RemoveStaffSchedule(int scheduleId)
    {
        var schedule = _weeklySchedule.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Could not find schedule.");
        }

        _weeklySchedule.Remove(schedule);
    }

    public StoreProfessionalException AddStaffException(DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {
        var exception = StoreProfessionalException.Create(StoreId, ProfessionalId, date, startTime, endTime, reason);

        if (exception.IsDayOff)
        {
            bool isPartiallyWorking = _professionalExceptions.Any(
                e => e.Date == date &&
                !e.IsDayOff
            );

            if (isPartiallyWorking)
            {
                throw new DomainException("Cannot schedule staff to partially work on a day off.");
            }
        }
        else
        {
            bool isOverlapping = _professionalExceptions.Any(
                e => e.Date == date &&
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

        _professionalExceptions.Add(exception);
        return exception;
    }

    public void RemoveStaffException(int exceptionId)
    {
        var exception = _professionalExceptions.FirstOrDefault(e => e.Id == exceptionId);

        if (exception == null)
        {
            throw new DomainException("Exception not found.");
        }

        _professionalExceptions.Remove(exception);
    }

    public bool IsAvailable(int professionalId, DateTime startAt, DateTime endAt)
    {
        var day = startAt.DayOfWeek;

        var daySchedules = _weeklySchedule.Where(s => s.Day == startAt.DayOfWeek && s.IsWorking).ToList();

        if (!daySchedules.Any())
        {
            return false;
        }

        bool isWithinSchedule = daySchedules.Any(s => startAt.TimeOfDay >= s.StartTime!.Value && endAt.TimeOfDay <= s.EndTime!.Value);
        if (!isWithinSchedule)
        {
            return false;
        }

        var dateExceptions = _professionalExceptions.Where(e => e.Date.Date == startAt.Date).ToList();

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