using Domain.Exceptions;

namespace Domain.Entities;

public class StaffCalendar
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private readonly List<StoreEmployeeSchedule> _schedules = new();
    public IReadOnlyCollection<StoreEmployeeSchedule> Schedules => _schedules.AsReadOnly();

    private readonly List<StoreEmployeeScheduleSpecial> _exceptions = new();
    public IReadOnlyCollection<StoreEmployeeScheduleSpecial> Exceptions => _exceptions.AsReadOnly();

    private StaffCalendar() { }

    public StaffCalendar Create(int storeId, int professionalId)
    {
        return new StaffCalendar
        {
            StoreId = storeId,
            ProfessionalId = professionalId
        };
    }

    public StoreEmployeeSchedule AddStaffSchedule(DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        var schedule = StoreEmployeeSchedule.Create(StoreId, ProfessionalId, day, startTime, endTime);

        if (schedule.IsTimeOff)
        {
            bool isWorking = _schedules.Any(s => s.Day == day && s.IsWorking);

            if (isWorking)
            {
                throw new DomainException("Cannot create full-day off when partial schedule exists.");
            }
        }
        else
        {
            bool isOverlapping = _schedules.Any(
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

    public StoreEmployeeScheduleSpecial AddStaffException(DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {

        var exception = StoreEmployeeScheduleSpecial.Create(StoreId, ProfessionalId, date, startTime, endTime, reason);

        if (exception.IsDayOff)
        {
            bool isPartiallyWorking = _exceptions.Any(
                e => e.Date == date &&
                !e.IsDayOff
            );

            if (isPartiallyWorking)
            {
                throw new DomainException("Cannot schedule an employe to work on a day off.");
            }
        }
        else
        {
            bool isOverlapping = _exceptions.Any(
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

    public bool IsProfessionalAvailable(DateTime startAt, DateTime endAt)
    {
        var day = startAt.DayOfWeek;

        var schedules = _schedules.Where(s => s.Day == day && s.IsWorking).ToList();

        if (schedules.Count == 0)
        {
            return false;
        }

        bool isWithinSchedule = schedules.Any(
            s => s.StartTime.HasValue &&
            s.EndTime.HasValue &&
            startAt.TimeOfDay >= s.StartTime.Value &&
            endAt.TimeOfDay <= s.EndTime.Value);
        if (!isWithinSchedule)
        {
            return false;
        }

        var exceptions = _exceptions.Where(e => e.Date.Date == startAt.Date).ToList();

        foreach (var exception in exceptions)
        {
            if (exception.IsBlocking(startAt, endAt))
            {
                return false;
            }
        }

        return true;
    }
}