using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreProfessionalSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public StoreProfessionalSchedule Create(int storeId, int professionalId, DayOfWeek dayOfWeek, TimeSpan start, TimeSpan end)
    {
        ValidateStoreProfessionalScheduleInfo(start, end);

        return new StoreProfessionalSchedule
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
            DayOfWeek = dayOfWeek,
            StartTime = start,
            EndTime = end
        };
    }

    private static void ValidateStoreProfessionalScheduleInfo(TimeSpan start, TimeSpan end)
    {
        if (start >= end)
        {
            throw new DomainException("Start time must be before end time.");
        }
    }
}