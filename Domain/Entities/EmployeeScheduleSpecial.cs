using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class EmployeeScheduleSpecial : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan? StartTime { get; private set; }
    public TimeSpan? EndTime { get; private set; }
    public string? Reason { get; private set; }

    private EmployeeScheduleSpecial() { }

    public static EmployeeScheduleSpecial Create(int storeId, int professionalId, DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {
        ValidateExceptionInfo(date, startTime, endTime, reason);

        return new EmployeeScheduleSpecial
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
            Date = date.Date,
            StartTime = startTime,
            EndTime = endTime,
            Reason = reason?.Trim()
        };
    }

    private static void ValidateExceptionInfo(DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {
        if ((startTime.HasValue && !endTime.HasValue) || (!startTime.HasValue && endTime.HasValue))
        {
            throw new DomainException("Both start and end times must be provided.");
        }

        if (startTime.HasValue && endTime.HasValue && startTime >= endTime)
        {
            throw new DomainException("Start time must be earlier than end time.");
        }

        if (date.Date < DateTime.UtcNow.Date)
        {
            throw new DomainException("Cannot create an exception for a past date.");
        }

        if (reason?.Length > 500)
        {
            throw new DomainException("Reason cannot be longer than 500 characters.");
        }
    }

    public bool IsWorkingDay => StartTime.HasValue || EndTime.HasValue;

    public bool IsDayOff => !IsWorkingDay;

    public bool IsBlocking(DateTime startAt, DateTime endAt)
    {
        if (startAt.Date != Date)
        {
            return false;
        }

        if (!IsWorkingDay)
        {
            return true;
        }

        var excStart = StartTime!.Value;
        var excEnd = EndTime!.Value;

        var appointmentStart = startAt.TimeOfDay;
        var appointmentEnd = endAt.TimeOfDay;

        return appointmentStart < excEnd && appointmentEnd > excStart;
    }
}