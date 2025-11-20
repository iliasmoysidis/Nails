using System.Reflection.Metadata.Ecma335;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreProfessionalSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeSpan? StartTime { get; private set; }
    public TimeSpan? EndTime { get; private set; }

    public static StoreProfessionalSchedule Create(int storeId, int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        ValidateStoreProfessionalScheduleInfo(startTime, endTime);

        return new StoreProfessionalSchedule
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
            Day = day,
            StartTime = startTime,
            EndTime = endTime
        };
    }

    private static void ValidateStoreProfessionalScheduleInfo(TimeSpan? startTime = null, TimeSpan? endTime = null)
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

    public bool IsWorkingDay => StartTime.HasValue && EndTime.HasValue;
}