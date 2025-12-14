using System.Reflection.Metadata.Ecma335;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class EmployeeSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeSpan? StartTime { get; private set; }
    public TimeSpan? EndTime { get; private set; }

    public static EmployeeSchedule Create(int storeId, int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        ValidateStoreStaffScheduleInfo(startTime, endTime);

        return new EmployeeSchedule
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
            Day = day,
            StartTime = startTime,
            EndTime = endTime
        };
    }

    private static void ValidateStoreStaffScheduleInfo(TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        if ((startTime.HasValue && !endTime.HasValue) || (!startTime.HasValue && endTime.HasValue))
        {
            throw new DomainException("Both start and end times must be provided.");
        }

        if (startTime.HasValue && endTime.HasValue && startTime >= endTime)
        {
            throw new DomainException("Start time must be before end time.");
        }
    }

    public bool IsWorking => StartTime.HasValue && EndTime.HasValue;

    public bool IsTimeOff => !IsWorking;
}